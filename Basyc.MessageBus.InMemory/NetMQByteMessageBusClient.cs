using Basyc.MessageBus.Client.NetMQ.Sessions;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serializaton.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using OneOf;
using System.Text;

namespace Basyc.MessageBus.Client.NetMQ;

//https://zguide.zeromq.org/docs/chapter3/#A-Load-Balancing-Message-Broker
public partial class NetMQByteMessageBusClient : IByteMessageBusClient
{
	private readonly IOptions<NetMQMessageBusClientOptions> options;
	private readonly IMessageHandlerManager handlerManager;
	private readonly ILogger<NetMQByteMessageBusClient> logger;
	private readonly ISessionManager<NetMQSessionResult> sessionManager;
	private readonly INetMQByteMessageSerializer netMQByteSerializer;
	private readonly NetMQPoller poller = new NetMQPoller();
	private readonly DealerSocket dealerSocket;

	public NetMQByteMessageBusClient(
		IOptions<NetMQMessageBusClientOptions> options,
		IMessageHandlerManager handlerManager,
		ILogger<NetMQByteMessageBusClient> logger,
		ISessionManager<NetMQSessionResult> sessionManager,
		INetMQByteMessageSerializer netMQByteSerializer)
	{
		this.options = options;
		this.handlerManager = handlerManager;
		this.logger = logger;
		this.sessionManager = sessionManager;
		this.netMQByteSerializer = netMQByteSerializer;

		dealerSocket = new DealerSocket($"tcp://{options.Value.BrokerServerAddress}:{options.Value.BrokerServerPort}");
		if (options.Value.WorkerId is null)
		{
			options.Value.WorkerId = "Client-" + Guid.NewGuid();
		}

		var clientIdBytes = Encoding.ASCII.GetBytes(options.Value.WorkerId);
		dealerSocket.Options.Identity = clientIdBytes;

		dealerSocket.ReceiveReady += async (s, a) =>
		{
			await DealerHandleMessage(CancellationToken.None);
		};

		poller.Add(dealerSocket);
	}



	public Task StartAsync(CancellationToken cancellationToken = default)
	{
		poller.RunAsync();
		CheckInMessage checkIn = new CheckInMessage(options.Value.WorkerId!, handlerManager.GetConsumableMessageTypes());
		var seriMessage = netMQByteSerializer.Serialize(checkIn, TypedToSimpleConverter.ConvertTypeToSimple(typeof(CheckInMessage)), default, MessageCase.CheckIn);
		var messageToServer = new NetMQMessage();
		messageToServer.AppendEmptyFrame();
		messageToServer.Append(seriMessage);
		logger.LogInformation($"Sending CheckIn message");
		dealerSocket.SendMultipartMessage(messageToServer);
		logger.LogInformation($"CheckIn message sent");
		return Task.CompletedTask;
	}

	private async Task DealerHandleMessage(CancellationToken cancellationToken)
	{
		var messageFrames = dealerSocket.ReceiveMultipartMessage(3);
		var senderAddressBytes = messageFrames[1].Buffer;
		var senderAddressString = Encoding.ASCII.GetString(senderAddressBytes);
		byte[] messageDataBytes = messageFrames[3].Buffer;

		var deserializationResult = netMQByteSerializer.Deserialize(messageDataBytes);
		deserializationResult.Switch(
			checkIn => logger.LogError("Client is not supposed to receive CheckIn messages"),
			async requestCase =>
			{
				logger.LogDebug($"Request received from {senderAddressString}:{requestCase.SessionId}, data: '{requestCase.RequestData}'");
				var consumeResult = await handlerManager.ConsumeMessage(requestCase.RequestType, requestCase.RequestData, cancellationToken);
				object connsumerResultData;
				if (consumeResult.Value is Exception ex)
				{
					logger.LogCritical($"Message handler throwed exception. {ex.Message}");
					connsumerResultData = ex;
				}
				connsumerResultData = consumeResult.AsT0;

				string responseType = TypedToSimpleConverter.ConvertTypeToSimple(connsumerResultData.GetType());
				byte[] responseBytes = netMQByteSerializer.Serialize(connsumerResultData, responseType, requestCase.SessionId, MessageCase.Response);
				var messageToServer = new NetMQMessage();
				messageToServer.AppendEmptyFrame();
				messageToServer.Append(responseBytes);
				messageToServer.AppendEmptyFrame();
				messageToServer.Append(senderAddressBytes);

				logger.LogInformation($"Sending response message");
				dealerSocket.SendMultipartMessage(messageToServer);
				logger.LogInformation($"Response message sent");
			},
			responseCase =>
			{
				logger.LogInformation($"Response received from {senderAddressString}:{responseCase.SessionId}, data: {responseCase.ResponseBytes}");
				if (sessionManager.TryCompleteSession(responseCase.SessionId, new NetMQSessionResult(responseCase.ResponseBytes, responseCase.ResponseType)) is false)
					logger.LogError($"Session '{responseCase.SessionId}' completation failed. Session does not exist");
			},
			async eventCase =>
			{
				logger.LogInformation($"Event received from {senderAddressString}:{eventCase.SessionId}, data: '{eventCase.EventData}'");
				var responseData = await handlerManager.ConsumeMessage(eventCase.EventType, eventCase.EventData, cancellationToken);
			},
			failureCase =>
			{
				logger.LogError($"Failure received from {senderAddressString}:{failureCase.SessionId}, data: '{failureCase}'");
				switch (failureCase.MessageCase)
				{
					case MessageCase.Response:
						CraeteErrorMessageBytes(failureCase.ErrorMessage, out string errorResultType, out byte[] errorResultBytes);
						if (sessionManager.TryCompleteSession(failureCase.SessionId, new NetMQSessionResult(errorResultBytes, errorResultType)) is false)
							logger.LogCritical($"Session '{failureCase.SessionId}' failed. Session does not exist");
						break;
					default:
						throw new NotImplementedException();
				}
			});

	}
	private Task PublishAsync(byte[]? eventBytes, string eventType, CancellationToken cancellationToken)
	{
		eventBytes ??= new byte[0];
		Task<NetMQSessionResult> task = Task.Run(() =>
		{
			cancellationToken.ThrowIfCancellationRequested();
			var newSession = sessionManager.CreateSession(eventType);
			var netMQByteMessage = netMQByteSerializer.Serialize(eventBytes, eventType, newSession.SessionId, MessageCase.Event);
			var messageToBroker = new NetMQMessage();
			messageToBroker.AppendEmptyFrame();
			messageToBroker.Append(netMQByteMessage);

			cancellationToken.ThrowIfCancellationRequested();

			logger.LogInformation($"Publishing '{eventType}' session: '{newSession.SessionId}'");
			try
			{
				dealerSocket.SendMultipartMessage(messageToBroker);
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to send request");
				CraeteErrorMessageBytes("Failed to publish", out string errorResultType, out byte[] errorResultBytes);
				sessionManager.TryCompleteSession(newSession.SessionId, new NetMQSessionResult(errorResultBytes, errorResultType));
			}

			logger.LogInformation($"Published '{eventType}'");
			var publishResult = "Published";
			var publisResultType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(string));
			var publishResultBytes = netMQByteSerializer.Serialize(publishResult, publisResultType, 0, MessageCase.Response);
			sessionManager.TryCompleteSession(newSession.SessionId, new NetMQSessionResult(publishResultBytes, publisResultType));
			return newSession.ResponseSource.Task;
		});

		return task;
	}

	private void CraeteErrorMessageBytes(string errorMessage, out string errorResultType, out byte[] errorResultBytes)
	{
		var errorResult = new ErrorMessage(errorMessage);
		errorResultType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(ErrorMessage));
		errorResultBytes = netMQByteSerializer.Serialize(errorResult, errorResultType, 0, MessageCase.Response);
	}

	private Task SendAsync(byte[]? commnadData, string commandType, CancellationToken cancellationToken)
	{
		return RequestAsync(commnadData, commandType, cancellationToken);
	}

	private async Task<OneOf<ByteResponse, ErrorMessage>> RequestAsync(byte[]? requestBytes, string requestType, CancellationToken cancellationToken)
	{
		requestBytes ??= new byte[0];

		Task<ByteResponse> task = Task.Run(async () =>
		{
			cancellationToken.ThrowIfCancellationRequested();
			var newSession = sessionManager.CreateSession(requestType);
			var netMQByteMessage = netMQByteSerializer.Serialize(requestBytes, requestType, newSession.SessionId, MessageCase.Request);
			var messageToBroker = new NetMQMessage();
			messageToBroker.AppendEmptyFrame();
			messageToBroker.Append(netMQByteMessage);

			cancellationToken.ThrowIfCancellationRequested();

			logger.LogInformation($"Requesting '{requestType}'");
			try
			{
				dealerSocket.SendMultipartMessage(messageToBroker);
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to send request");
				CraeteErrorMessageBytes("Failed to send request", out string errorResultType, out byte[] errorResultBytes);
				sessionManager.TryCompleteSession(newSession.SessionId, new NetMQSessionResult(errorResultBytes, errorResultType));
			}

			logger.LogInformation($"Requested '{requestType}'");
			var sessionResult = await newSession.ResponseSource.Task;
			return new ByteResponse(sessionResult.bytes, sessionResult.responseType);
		});

		return await task;
	}
	Task IByteMessageBusClient.PublishAsync(string eventType, CancellationToken cancellationToken)
	{
		return PublishAsync(null, eventType, cancellationToken);
	}

	Task IByteMessageBusClient.PublishAsync(string eventType, byte[] eventBytes, CancellationToken cancellationToken)
	{
		return PublishAsync(eventBytes, eventType, cancellationToken);
	}

	Task IByteMessageBusClient.SendAsync(string commandType, CancellationToken cancellationToken)
	{
		return SendAsync(null, commandType, cancellationToken);
	}

	Task IByteMessageBusClient.SendAsync(string commandType, byte[] commandData, CancellationToken cancellationToken)
	{
		return SendAsync(commandData, commandType, cancellationToken);
	}

	async Task<ByteResponse> IByteMessageBusClient.RequestAsync(string requestType, CancellationToken cancellationToken)
	{
		var response = await RequestAsync(null, requestType, cancellationToken);
		return response.Match(byteResponse => byteResponse, error => throw new Exception(error.Message));
	}

	async Task<OneOf<ByteResponse, ErrorMessage>> IByteMessageBusClient.RequestAsync(string requestType, byte[] requestData, CancellationToken cancellationToken)
	{
		return await RequestAsync(requestData, requestType, cancellationToken);
	}

	public void Dispose()
	{
		dealerSocket.Dispose();
		poller.Dispose();
	}

}
