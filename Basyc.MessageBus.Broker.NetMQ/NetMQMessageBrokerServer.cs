using Basyc.MessageBus.NetMQ.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serializaton.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using System.Text;

namespace Basyc.MessageBus.Broker.NetMQ;

//https://netmq.readthedocs.io/en/latest/xpub-xsub/
public class NetMQMessageBrokerServer : IMessageBrokerServer
{
    private readonly IOptions<NetMQMessageBrokerServerOptions> options;
    private readonly IWorkerRegistry workerRegistry;
    private readonly NetMQPoller poller = new NetMQPoller();
    private readonly ILogger<NetMQMessageBrokerServer> logger;
    private readonly IMessageToByteSerializer messageToByteSerializer;
    private readonly RouterSocket brokerSocket;

    public NetMQMessageBrokerServer(IOptions<NetMQMessageBrokerServerOptions> options,
        IWorkerRegistry workerRegistry,
        ILogger<NetMQMessageBrokerServer> logger,
        IMessageToByteSerializer messageToByteSerializer
        )
    {
        this.options = options;
        this.workerRegistry = workerRegistry;
        this.logger = logger;
        this.messageToByteSerializer = messageToByteSerializer;
        brokerSocket = new RouterSocket($"@tcp://{options.Value.BrokerServerAddress}:{options.Value.BrokerServerPort}");

        brokerSocket.ReceiveReady += (s, a) =>
        {
            var recievedMessageFrame = a.Socket.ReceiveMultipartMessage(3);
            //0 WorkerId 1 Empty 2 Data 3 Empty 4 ProducerId

            var senderAddressFrame = recievedMessageFrame[0];
            var senderAddressString = senderAddressFrame.ConvertToString();
            var deserializationResult = messageToByteSerializer.Deserialize(recievedMessageFrame[2].Buffer);
            deserializationResult.Switch(
                checkIn =>
                {
                    logger.LogInformation($"CheckIn received {checkIn} from {senderAddressString}");
                    workerRegistry.RegisterWorker(checkIn.WorkerId, checkIn.SupportedMessageTypes);
                },
                request =>
                {
                    logger.LogInformation($"Recieved request: '{request.RequestData}' from {senderAddressString}:{request.SessionId}");
                    if (workerRegistry.TryGetWorkerFor(request.RequestType, out string? workerAddressString))
                    {
                        byte[] requestBytes = recievedMessageFrame[2].Buffer;
                        var messageToProducer = new NetMQMessage();
                        messageToProducer.Append(workerAddressString!);
                        messageToProducer.AppendEmptyFrame();
                        messageToProducer.Append(senderAddressFrame);
                        messageToProducer.AppendEmptyFrame();
                        messageToProducer.Append(requestBytes);
                        logger.LogDebug($"Sending request: '{request.RequestData}' to {workerAddressString}:{request.SessionId}");
                        brokerSocket.SendMultipartMessage(messageToProducer);
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
                        messageToProducer.Append(messageToByteSerializer.Serialize(failure, TypedToSimpleConverter.ConvertTypeToSimple(typeof(ErrorMessage)), request.SessionId, MessageCase.Response));
                        logger.LogError($"Sending failure: '{failure}' to {senderAddressString}");
                        brokerSocket.SendMultipartMessage(messageToProducer);
                        logger.LogError($"Failure sent to {senderAddressFrame}");
                    }
                },
                response =>
                {
                    logger.LogInformation($"Response recieved: '{response.ReponseData}' from {senderAddressString}:{response.SessionId}");
                    var messageToConsumer = new NetMQMessage();
                    var producerAddress = recievedMessageFrame[4];
                    var producerAddressString = producerAddress.ConvertToString();
                    messageToConsumer.Append(producerAddress);
                    messageToConsumer.AppendEmptyFrame();
                    messageToConsumer.Append(senderAddressFrame);
                    messageToConsumer.AppendEmptyFrame();
                    messageToConsumer.Append(recievedMessageFrame[2].Buffer);
                    logger.LogDebug($"Sending response: '{response.ReponseData}' to {producerAddressString}:{response.SessionId}");
                    brokerSocket.SendMultipartMessage(messageToConsumer);
                    logger.LogDebug($"Response sent to {producerAddressString}");
                },
                @event =>
                {
                    logger.LogInformation($"Event recieved {@event} from {senderAddressString}");

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
                            logger.LogDebug($"Sending event: '{@event.EventData}' to {worker}:{@event.SessionId}");
                            brokerSocket.SendMultipartMessage(messageToProducer);
                            logger.LogDebug($"Event sent to {worker}");
                        }
                    }
                    else
                    {
                        logger.LogInformation($"No worker for {@event.EventType} checked in");
                    }
                },
                failure =>
                {
                    logger.LogError($"Serialization failed, details: '{failure}'");
                    var sendFailToAddress = failure.MessageCase is MessageCase.Response ? recievedMessageFrame[4] : senderAddressFrame;
                    string sendFailToAddressString = sendFailToAddress.ConvertToString();
                    var failResult = new ErrorMessage(failure.ErrorMessage);
                    var messageToProducer = new NetMQMessage();
                    messageToProducer.Append(sendFailToAddress);
                    messageToProducer.AppendEmptyFrame();
                    messageToProducer.AppendEmptyFrame();
                    messageToProducer.AppendEmptyFrame();
                    messageToProducer.Append(messageToByteSerializer.Serialize(failResult, TypedToSimpleConverter.ConvertTypeToSimple(typeof(ErrorMessage)), failure.SessionId, MessageCase.Response));
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


}
