using Basyc.MessageBus.Client.NetMQ.Sessions;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
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
	private readonly INetMQMessageWrapper netMQMessageWrapper;
	private readonly IObjectToByteSerailizer objectToByteSerailizer;
	private readonly NetMQPoller poller = new();
	private readonly DealerSocket dealerSocket;

	public NetMQByteMessageBusClient(
		IOptions<NetMQMessageBusClientOptions> options,
		IMessageHandlerManager handlerManager,
		ILogger<NetMQByteMessageBusClient> logger,
		ISessionManager<NetMQSessionResult> sessionManager,
		INetMQMessageWrapper netMQByteSerializer,
		IObjectToByteSerailizer objectToByteSerailizer)
	{
		this.options = options;
		this.handlerManager = handlerManager;
		this.logger = logger;
		this.sessionManager = sessionManager;
		this.netMQMessageWrapper = netMQByteSerializer;
		this.objectToByteSerailizer = objectToByteSerailizer;

		dealerSocket = CreateSocket(options);
		poller.Add(dealerSocket);
	}

	private DealerSocket CreateSocket(IOptions<NetMQMessageBusClientOptions> options)
	{
		var dealerSocket = new DealerSocket($"tcp://{options.Value.BrokerServerAddress}:{options.Value.BrokerServerPort}");

		if (options.Value.WorkerId is null)
			options.Value.WorkerId = "Client-" + Guid.NewGuid();

		dealerSocket.Options.Identity = Encoding.ASCII.GetBytes(options.Value.WorkerId);
		dealerSocket.ReceiveReady += async (s, a) =>
		{
			var messageFrames = dealerSocket.ReceiveMultipartMessage(3);
			//await HandleMessage(messageFrames,CancellationToken.None);
			await Task.Run(async () =>
			{
				await HandleMessage(messageFrames, CancellationToken.None);
			});
		};

		return dealerSocket;
	}

	public Task StartAsync(CancellationToken cancellationToken = default)
	{
		poller.RunAsync();
		CheckInMessage checkIn = new CheckInMessage(options.Value.WorkerId!, handlerManager.GetConsumableMessageTypes());
		byte[] messageWrapperBytes = netMQMessageWrapper.CreateWrapperMessage(checkIn, TypedToSimpleConverter.ConvertTypeToSimple(typeof(CheckInMessage)), default, MessageCase.CheckIn);
		var messageToServer = new NetMQMessage();
		messageToServer.AppendEmptyFrame();
		messageToServer.Append(messageWrapperBytes);
		logger.LogInformation($"Sending CheckIn message");
		dealerSocket.SendMultipartMessage(messageToServer);
		logger.LogInformation($"CheckIn message sent");
		return Task.CompletedTask;
	}

	private async Task HandleMessage(NetMQMessage messageFrames, CancellationToken cancellationToken)
	{
		//NetMQMessage messageFrames = dealerSocket.ReceiveMultipartMessage(3);
		var senderAddressBytes = messageFrames[1].Buffer;
		var senderAddressString = Encoding.ASCII.GetString(senderAddressBytes);
		byte[] messageDataBytes = messageFrames[3].Buffer;

		var wrapperReadResult = netMQMessageWrapper.ReadWrapperMessage(messageDataBytes);
		await wrapperReadResult.Match(
			checkIn =>
			{
				logger.LogError("Client is not supposed to receive CheckIn messages");
				return Task.CompletedTask;
			},
			async requestCase =>
			{
				logger.LogDebug($"Request received from {senderAddressString}:{requestCase.SessionId}, data: '{requestCase.RequestBytes}'");
				var deserializedRequest = objectToByteSerailizer.Deserialize(requestCase.RequestBytes, requestCase.RequestType);
				var consumeResult = await handlerManager.ConsumeMessage(requestCase.RequestType, deserializedRequest, cancellationToken, requestCase.SessionId);
				object connsumerResultData;
				if (consumeResult.Value is Exception ex)
				{
					logger.LogCritical($"Message handler throwed exception. {ex.Message}");
					connsumerResultData = ex;
				}
				else
				{
					connsumerResultData = consumeResult.AsT0;
				}

				string responseType = TypedToSimpleConverter.ConvertTypeToSimple(connsumerResultData.GetType());
				byte[] wrapperMessageBytes = netMQMessageWrapper.CreateWrapperMessage(connsumerResultData, responseType, requestCase.SessionId, MessageCase.Response);
				var messageToServer = new NetMQMessage();
				messageToServer.AppendEmptyFrame();
				messageToServer.Append(wrapperMessageBytes);
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

				return Task.CompletedTask;
			},
			async eventCase =>
			{
				logger.LogInformation($"Event received from {senderAddressString}:{eventCase.SessionId}, data: '{eventCase.EventBytes}'");
				var eventRequest = objectToByteSerailizer.Deserialize(eventCase.EventBytes, eventCase.EventType);
				var responseData = await handlerManager.ConsumeMessage(eventCase.EventType, eventRequest, cancellationToken, eventCase.SessionId);
				//var sessionResult = CreateEventPublishedMessageBytes();
				//if (sessionManager.TryCompleteSession(eventCase.SessionId, sessionResult) is false)
				//	logger.LogError($"Session '{eventCase.SessionId}' completation failed. Session does not exist");
			},
			failureCase =>
			{
				logger.LogError($"Failure received from {senderAddressString}:{failureCase.SessionId}, data: '{failureCase}'");
				switch (failureCase.MessageCase)
				{
					case MessageCase.Response:
						var sessionResult = CreateErrorMessageBytes(failureCase.ErrorMessage);
						if (sessionManager.TryCompleteSession(failureCase.SessionId, sessionResult) is false)
							logger.LogCritical($"Session '{failureCase.SessionId}' failed. Session does not exist");
						break;
					default:
						throw new NotImplementedException();
				}
				return Task.CompletedTask;

			});

	}


	private Task PublishAsync(byte[]? eventBytes, string eventType, CancellationToken cancellationToken)
	{
		var task = Task.Run(() =>
		{
			eventBytes ??= new byte[0];
			cancellationToken.ThrowIfCancellationRequested();
			var newSession = sessionManager.CreateSession(eventType);
			var netMQByteMessage = netMQMessageWrapper.CreateWrapperMessage(eventBytes, eventType, newSession.SessionId, MessageCase.Event);
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
				var sessionResult = CreateErrorMessageBytes("Failed to publish");
				sessionManager.TryCompleteSession(newSession.SessionId, sessionResult);
			}

			logger.LogInformation($"Published '{eventType}'");
			var publishResult = "Published";
			var publisResultType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(string));
			var publishResultBytes = netMQMessageWrapper.CreateWrapperMessage(publishResult, publisResultType, 0, MessageCase.Response);
			sessionManager.TryCompleteSession(newSession.SessionId, new NetMQSessionResult(publishResultBytes, publisResultType));
			//return newSession.ResponseSource.Task;
			//return Task.CompletedTask;
		});

		return task;
	}

	private NetMQSessionResult CreateErrorMessageBytes(string errorMessage)
	{
		var errorResult = new ErrorMessage(errorMessage);
		var errorResultType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(ErrorMessage));
		var errorResultBytes = netMQMessageWrapper.CreateWrapperMessage(errorResult, errorResultType, 0, MessageCase.Response);
		return new NetMQSessionResult(errorResultBytes, errorResultType);
	}

	//private NetMQSessionResult CreateEventPublishedMessageBytes()
	//{
	//	var voidResult = new VoidResult();
	//	var resultType = TypedToSimpleConverter.ConvertTypeToSimple(typeof(VoidResult));
	//	var resultBytes = netMQMessageWrapper.CreateWrapperMessage(voidResult, resultType, 0, MessageCase.Response);
	//	return new NetMQSessionResult(resultBytes, resultType);
	//}

	private Task SendAsync(byte[]? commnadData, string commandType, CancellationToken cancellationToken)
	{
		return RequestAsync(commnadData, commandType, cancellationToken).Task;
	}

	//private Task<OneOf<ByteResponse, ErrorMessage>> RequestAsync(byte[]? requestBytes, string requestType, CancellationToken cancellationToken)
	//{
	//	Task<OneOf<ByteResponse, ErrorMessage>> task = Task.Run<OneOf<ByteResponse, ErrorMessage>>(async () =>
	//	{
	//		requestBytes ??= new byte[0];
	//		cancellationToken.ThrowIfCancellationRequested();
	//		var newSession = sessionManager.CreateSession(requestType);
	//		var netMQByteMessage = netMQMessageWrapper.CreateWrapperMessage(requestBytes, requestType, newSession.SessionId, MessageCase.Request);
	//		var messageToBroker = new NetMQMessage();
	//		messageToBroker.AppendEmptyFrame();
	//		messageToBroker.Append(netMQByteMessage);

	//		cancellationToken.ThrowIfCancellationRequested();

	//		logger.LogInformation($"Requesting '{requestType}'");
	//		try
	//		{
	//			dealerSocket.SendMultipartMessage(messageToBroker);
	//		}
	//		catch (Exception ex)
	//		{
	//			logger.LogCritical(ex, "Failed to send request");
	//			var sessionResultError = CreateErrorMessageBytes("Failed to send request");
	//			sessionManager.TryCompleteSession(newSession.SessionId, sessionResultError);
	//		}

	//		logger.LogInformation($"Requested '{requestType}'");
	//		var sessionResult = await newSession.ResponseSource.Task;
	//		return new ByteResponse(sessionResult.bytes, sessionResult.responseType);
	//	});

	//	return task;
	//}

	private BusTask<ByteResponse> RequestAsync(byte[]? requestBytes, string requestType, CancellationToken cancellationToken)
	{
		var newSession = sessionManager.CreateSession(requestType);

		Task<OneOf<ByteResponse, ErrorMessage>> task = Task.Run<OneOf<ByteResponse, ErrorMessage>>(async () =>
		{
			requestBytes ??= new byte[0];
			cancellationToken.ThrowIfCancellationRequested();
			var netMQByteMessage = netMQMessageWrapper.CreateWrapperMessage(requestBytes, requestType, newSession.SessionId, MessageCase.Request);
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
				var sessionResultError = CreateErrorMessageBytes("Failed to send request");
				sessionManager.TryCompleteSession(newSession.SessionId, sessionResultError);
			}

			logger.LogInformation($"Requested '{requestType}'");
			var sessionResult = await newSession.ResponseSource.Task;
			return new ByteResponse(sessionResult.bytes, sessionResult.responseType);
		});

		return BusTask<ByteResponse>.FromTask(newSession.SessionId, task);
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
		var response = await RequestAsync(null, requestType, cancellationToken).Task;
		return response.Match(byteResponse => byteResponse, error => throw new Exception(error.Message));
	}

	//async Task<OneOf<ByteResponse, ErrorMessage>> IByteMessageBusClient.RequestAsync(string requestType, byte[] requestData, CancellationToken cancellationToken)
	//{
	//	return await RequestAsync(requestData, requestType, cancellationToken);
	//}

	BusTask<ByteResponse> IByteMessageBusClient.RequestAsync(string requestType, byte[] requestData, CancellationToken cancellationToken)
	{
		var reqeustTask = RequestAsync(requestData, requestType, cancellationToken);
		return reqeustTask;

	}

	public void Dispose()
	{
		dealerSocket.Dispose();
		poller.Dispose();
	}

}
