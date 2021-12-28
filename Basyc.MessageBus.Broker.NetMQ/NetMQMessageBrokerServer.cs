using Basyc.MessageBus.NetMQ.Shared;
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
    private readonly XPublisherSocket publisherSocker;
    private readonly XSubscriberSocket subscriberSocket;
    private readonly RouterSocket producerRouterSocket;

    public NetMQMessageBrokerServer(IOptions<NetMQMessageBrokerServerOptions> options, IWorkerRegistry workerRegistry, ILogger<NetMQMessageBrokerServer> logger)
    {
        this.options = options;
        this.workerRegistry = workerRegistry;
        this.logger = logger;

        publisherSocker = new XPublisherSocket($"@tcp://{options.Value.AddressForSubscribers}:{options.Value.PortForSubscribers}");
        subscriberSocket = new XSubscriberSocket($"@tcp://{options.Value.AddressForPublishers}:{options.Value.PortForPublishers}");
        producerRouterSocket = new RouterSocket($"@tcp://{options.Value.BrokerServerAddress}:{options.Value.BrokerServerPort}");        

        producerRouterSocket.ReceiveReady += (s, a) =>
        {
            var recievedMessageFrame = a.Socket.ReceiveMultipartMessage(3);
            //0 WorkerId 1 Empty 2 Data 3 Empty 4 ProducerId

            var senderAddressFrame = recievedMessageFrame[0];
            var senderAddressString = senderAddressFrame.ConvertToString();
            try
            {
                DeserializedMessage deserializedMessage = TypedMessageToByteSerializer.Deserialize(recievedMessageFrame[2].Buffer);

                switch (deserializedMessage.MessageCase)
                {
                    case MessageCase.CheckIn:
                        if (recievedMessageFrame.FrameCount != 3)
                            logger.LogWarning($"CheckIn recieved with unexpected format");

                        var checkIn = deserializedMessage.CheckIn!;
                        logger.LogInformation($"CheckIn recieved {checkIn} from {senderAddressString}");
                        workerRegistry.RegisterWorker(checkIn.WorkerId, checkIn.SupportedMessageTypes);

                        break;

                    case MessageCase.Response:
                        if (recievedMessageFrame.FrameCount != 5)
                            logger.LogWarning($"Recieved response with unexpected format");

                        var responseCase = deserializedMessage.Response!;
                        logger.LogInformation($"Recieved response: '{responseCase.ReponseData}' from {senderAddressString}:{responseCase.SessionId}");
                        var messageToConsumer = new NetMQMessage();
                        var producerAddress = recievedMessageFrame[4];
                        var producerAddressString = producerAddress.ConvertToString();
                        messageToConsumer.Append(producerAddress);
                        messageToConsumer.AppendEmptyFrame();
                        messageToConsumer.Append(senderAddressFrame);
                        messageToConsumer.AppendEmptyFrame();
                        messageToConsumer.Append(recievedMessageFrame[2].Buffer);
                        logger.LogDebug($"Sending response: '{responseCase.ReponseData}' to {producerAddressString}:{responseCase.SessionId}");
                        producerRouterSocket.SendMultipartMessage(messageToConsumer);
                        logger.LogDebug($"Response sent to {producerAddressString}");

                        break;

                    case MessageCase.Request:
                        var requestCase = deserializedMessage.Request!;
                        logger.LogInformation($"Recieved request: '{requestCase.RequestData}' from {senderAddressString}:{requestCase.SessionId}");
                        if (workerRegistry.TryGetWorkerFor(requestCase.RequestType, out string? workerAddressString))
                        {
                            byte[] requestBytes = recievedMessageFrame[2].Buffer;
                            var messageToProducer = new NetMQMessage();
                            messageToProducer.Append(workerAddressString!);
                            messageToProducer.AppendEmptyFrame();
                            messageToProducer.Append(senderAddressFrame);
                            messageToProducer.AppendEmptyFrame();
                            messageToProducer.Append(requestBytes);
                            logger.LogDebug($"Sending request: '{requestCase.RequestData}' to {workerAddressString}:{requestCase.SessionId}");
                            producerRouterSocket.SendMultipartMessage(messageToProducer);
                            logger.LogDebug($"Request sent to {workerAddressString}");
                        }
                        else
                        {
                            var failure = new FailResult("Worker for this request not found!");
                            var messageToProducer = new NetMQMessage();
                            messageToProducer.Append(senderAddressFrame);
                            messageToProducer.AppendEmptyFrame();
                            messageToProducer.AppendEmptyFrame();
                            messageToProducer.AppendEmptyFrame();
                            messageToProducer.Append(TypedMessageToByteSerializer.Serialize(failure, requestCase.SessionId, true));
                            logger.LogError($"Sending failure: '{failure}' to {senderAddressString}");
                            producerRouterSocket.SendMultipartMessage(messageToProducer);
                            logger.LogError($"Failure sent to {senderAddressFrame}");
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                var failure = new FailResult(ex.Message);
                var messageToProducer = new NetMQMessage();
                messageToProducer.Append(senderAddressFrame);
                messageToProducer.AppendEmptyFrame();
                messageToProducer.AppendEmptyFrame();
                messageToProducer.AppendEmptyFrame();
                messageToProducer.Append(TypedMessageToByteSerializer.Serialize(failure, -1, true));
                logger.LogCritical($"Sending failure: '{failure}' to {senderAddressString}");
                producerRouterSocket.SendMultipartMessage(messageToProducer);
                logger.LogCritical($"Failure sent to {senderAddressFrame}");
            }

        };

        poller.Add(producerRouterSocket);
    }

    public void Start()
    {
        try
        {
            logger.LogDebug("Starting poller");
            poller.RunAsync();
            logger.LogDebug("poller started");
            Proxy proxy = new Proxy(subscriberSocket, publisherSocker);
            logger.LogDebug("PUB/SUB proxy starting");
            proxy.Start();
            logger.LogDebug("PUB/SUB proxy stopped");

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
        publisherSocker.Dispose();
        subscriberSocket.Dispose();
        producerRouterSocket.Dispose();
        logger.LogInformation("NetMQ proxy disposed");
    }


}
