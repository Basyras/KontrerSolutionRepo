using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Shared;
using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Helpers;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.NetMQ.Shared.Cases;
using Basyc.MessageBus.Shared;
using Basyc.Serializaton.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Basyc.MessageBus.Broker.NetMQ;

//https://netmq.readthedocs.io/en/latest/xpub-xsub/
public class NetMQMessageBrokerServer : IMessageBrokerServer
{
	private readonly IOptions<NetMQMessageBrokerServerOptions> options;
	private readonly IWorkerRegistry workerRegistry;
	private readonly NetMQPoller poller = new NetMQPoller();
	private readonly ILogger<NetMQMessageBrokerServer> logger;
	private readonly INetMQMessageWrapper messageToByteSerializer;
	private readonly IDiagnosticsExporter diagnosticsProducer;
	private readonly RouterSocket brokerSocket;

	public NetMQMessageBrokerServer(IOptions<NetMQMessageBrokerServerOptions> options,
		IWorkerRegistry workerRegistry,
		ILogger<NetMQMessageBrokerServer> logger,
		INetMQMessageWrapper messageToByteSerializer,
		IDiagnosticsExporter diagnosticsProducer
		)
	{
		this.options = options;
		this.workerRegistry = workerRegistry;
		this.logger = logger;
		this.messageToByteSerializer = messageToByteSerializer;
		this.diagnosticsProducer = diagnosticsProducer;
		brokerSocket = new RouterSocket($"@tcp://{options.Value.BrokerServerAddress}:{options.Value.BrokerServerPort}");

		brokerSocket.ReceiveReady += (s, a) =>
		{
			var recievedMessageFrame = a.Socket.ReceiveMultipartMessage(3);
			//0 WorkerId 1 Empty 2 Data 3 Empty 4 ProducerId

			var senderAddressFrame = recievedMessageFrame[0];
			var senderAddressString = senderAddressFrame.ConvertToString();
			var deserializationResult = messageToByteSerializer.ReadWrapperMessage(recievedMessageFrame[2].Buffer);
			deserializationResult.Switch(
				checkIn =>
				{
					string handledTypesNamesString = string.Join(',', checkIn.SupportedMessageTypes
						.Select(assemblyTypeString => assemblyTypeString.Split(',')
						.First()
						.Split('.')
						.Last()));
					logger.LogInformation($"CheckIn received from {senderAddressString}. WorkerId: {checkIn.WorkerId}, HandeledTypes: {handledTypesNamesString}");
					workerRegistry.RegisterWorker(checkIn.WorkerId, checkIn.SupportedMessageTypes);
				},
				request =>
				{
					using var requestStartActivity = DiagnosticHelper.Start("RequestCase handeling", request.TraceId, request.ParentSpanId);
					//var requestStartActivity = CreateActivity(request.TraceId, "RequestCase handeling");
					//diagnosticsProducer.StartActivity(requestStartActivity);

					//diagnosticsProducer.ProduceLog(new LogEntry(ServiceIdentity.ApplicationWideIdentity, request.TraceId, DateTimeOffset.UtcNow, LogLevel.Debug, "Delegating request", requestStartActivity.Id));

					logger.LogInformation($"Recieved request {request.RequestType} ({request.RequestBytes.Length} bytes) from {senderAddressString}:{request.SessionId}");
					if (workerRegistry.TryGetWorkerFor(request.RequestType, out string? workerAddressString))
					{
						byte[] requestBytes = recievedMessageFrame[2].Buffer;
						var messageToProducer = new NetMQMessage();
						messageToProducer.Append(workerAddressString!);
						messageToProducer.AppendEmptyFrame();
						messageToProducer.Append(senderAddressFrame);
						messageToProducer.AppendEmptyFrame();
						messageToProducer.Append(requestBytes);
						logger.LogDebug($"Sending request {request.RequestType} to {workerAddressString}:{request.SessionId}");
						brokerSocket.SendMultipartMessage(messageToProducer);
						//diagnosticsProducer.EndActivity(requestStartActivity);
						requestStartActivity.Stop();

						logger.LogDebug($"Request sent to {workerAddressString}");
					}
					else
					{
						var failure = new ErrorMessage("Worker for this request not found!");
						var messageToProducer = new NetMQMessage();
						messageToProducer.Append(senderAddressFrame);
						messageToProducer.AppendEmptyFrame();
						messageToProducer.AppendEmptyFrame();
						messageToProducer.AppendEmptyFrame();
						messageToProducer.Append(messageToByteSerializer.CreateWrapperMessage(failure, TypedToSimpleConverter.ConvertTypeToSimple(typeof(ErrorMessage)), request.SessionId, request.TraceId, request.ParentSpanId, MessageCase.Response));
						logger.LogError($"Sending failure: '{failure}' to {senderAddressString}");
						brokerSocket.SendMultipartMessage(messageToProducer);
						//diagnosticsProducer.EndActivity(requestStartActivity);
						requestStartActivity.Stop();

						logger.LogError($"Failure sent to {senderAddressFrame}");
					}
				},
				response =>
				{
					using var responseActivityStart = DiagnosticHelper.Start("ResponseCase handeling", response.TraceId, response.ParentSpanId);
					logger.LogDebug($"Response recieved: '{response.ResponseBytes}' from {senderAddressString}:{response.SessionId}");
					var messageToConsumer = new NetMQMessage();
					var producerAddress = recievedMessageFrame[4];
					var producerAddressString = producerAddress.ConvertToString();
					messageToConsumer.Append(producerAddress);
					messageToConsumer.AppendEmptyFrame();
					messageToConsumer.Append(senderAddressFrame);
					messageToConsumer.AppendEmptyFrame();
					messageToConsumer.Append(recievedMessageFrame[2].Buffer);
					logger.LogDebug($"Sending response: '{response.ResponseBytes}' to {producerAddressString}:{response.SessionId}");
					brokerSocket.SendMultipartMessage(messageToConsumer);
					responseActivityStart.Stop();
					logger.LogDebug($"Response sent to {producerAddressString}");

				},
				@event =>
				{
					using var eventStartActivity = DiagnosticHelper.Start($"EventCase handeling", @event.TraceId, @event.ParentSpanId);

					logger.LogInformation($"EventCase recieved {@event} from {senderAddressString}");

					if (workerRegistry.TryGetWorkersFor(@event.EventType, out string[] workers))
					{
						foreach (var worker in workers)
						{
							byte[] eventData = recievedMessageFrame[2].Buffer;
							var messageToProducer = new NetMQMessage();
							messageToProducer.Append(worker!);
							messageToProducer.AppendEmptyFrame();
							messageToProducer.Append(senderAddressFrame);
							messageToProducer.AppendEmptyFrame();
							messageToProducer.Append(eventData);
							logger.LogInformation($"Sending event: '{@event.EventBytes}' to {worker}:{@event.SessionId}");
							brokerSocket.SendMultipartMessage(messageToProducer);
							logger.LogInformation($"Event sent to {worker}");
						}
					}
					else
					{
						logger.LogInformation($"No worker for {@event.EventType} checked in");
					}
					eventStartActivity.Stop();
				},
				failure =>
				{
					logger.LogError($"Serialization failed, details: '{failure}'");
					var sendFailToAddress = failure.MessageCase is MessageCase.Response ? recievedMessageFrame[4] : senderAddressFrame;
					string sendFailToAddressString = sendFailToAddress.ConvertToString();
					var failResult = new ErrorMessage(failure.ErrorMessage);
					//var messageToProducer = new NetMQMessage();
					//messageToProducer.Append(sendFailToAddress);
					//messageToProducer.AppendEmptyFrame();
					//messageToProducer.AppendEmptyFrame();
					//messageToProducer.AppendEmptyFrame();
					//messageToProducer.Append(messageToByteSerializer.CreateWrapperMessage(failResult, TypedToSimpleConverter.ConvertTypeToSimple(typeof(ErrorMessage)), failure.SessionId, failure.TraceId, failure.ParentSpanId, MessageCase.Response));
					var messageToProducer = CreateResponseNetMQMessage(sendFailToAddress, failResult, failure.SessionId, failure.TraceId, failure.ParentSpanId, MessageCase.Response);
					logger.LogDebug($"Sending failure: '{failure}' to {sendFailToAddressString}");
					brokerSocket.SendMultipartMessage(messageToProducer);
					logger.LogDebug($"Failure sent to {sendFailToAddressString}");
				});

		};

		poller.Add(brokerSocket);
	}



	public void Start()
	{
		try
		{
			logger.LogDebug("Starting poller");
			poller.RunAsync();
			logger.LogDebug("poller started");

		}
		catch (Exception ex)
		{
			logger.LogDebug($"NetMQ proxy stopped. Reason: {ex.Message}");
			throw;
		}
	}
	public Task StartAsync(CancellationToken cancellationToken = default)
	{
		return Task.Run(Start, cancellationToken);
	}

	public void Dispose()
	{
		brokerSocket.Dispose();
		logger.LogInformation("NetMQ proxy disposed");
	}

	private ActivityStart CreateActivity(string traceId, string name)
	{
		return new ActivityStart(ServiceIdentity.ApplicationWideIdentity, traceId, null, IdGeneratorHelper.GenerateNewSpanId(), name, DateTimeOffset.UtcNow);
	}

	private NetMQMessage CreateResponseNetMQMessage<TData>(NetMQFrame addressToSend, TData data, int sessionId, string traceId, string parentSpanId, MessageCase messageCase)
	{
		var mqMessage = new NetMQMessage();
		mqMessage.Append(addressToSend);
		mqMessage.AppendEmptyFrame();
		mqMessage.AppendEmptyFrame();
		mqMessage.AppendEmptyFrame();
		mqMessage.Append(messageToByteSerializer.CreateWrapperMessage(data, TypedToSimpleConverter.ConvertTypeToSimple(typeof(TData)), sessionId, traceId, parentSpanId, messageCase));
		return mqMessage;
	}

	private NetMQMessage CreateRequestNetMQMessage<TData>(NetMQFrame addressToSend, NetMQFrame requestProducerAddress, TData data, int sessionId, string traceId, string parentSpanId, MessageCase messageCase)
	{
		var mqMessage = new NetMQMessage();
		mqMessage.Append(addressToSend);
		mqMessage.AppendEmptyFrame();
		mqMessage.Append(requestProducerAddress);
		mqMessage.AppendEmptyFrame();
		mqMessage.Append(messageToByteSerializer.CreateWrapperMessage(data, TypedToSimpleConverter.ConvertTypeToSimple(typeof(TData)), sessionId, traceId, parentSpanId, messageCase));
		return mqMessage;
	}
}
