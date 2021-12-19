using Basyc.MessageBus.NetMQ.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Basyc.MessageBus.Broker.NetMQ;

//https://netmq.readthedocs.io/en/latest/xpub-xsub/
public class NetMQMessageBrokerServer : IMessageBrokerServer
{
    private readonly IOptions<NetMQMessageBrokerServerOptions> options;
    private readonly NetMQPoller poller = new NetMQPoller();
    private readonly ILogger<NetMQMessageBrokerServer> logger;
    private readonly XPublisherSocket publisherSocker;
    private readonly XSubscriberSocket subscriberSocket;
    private readonly RouterSocket producerRouterSocket;
    private readonly RouterSocket consumerRouterSocket;

    public NetMQMessageBrokerServer(IOptions<NetMQMessageBrokerServerOptions> options, ILogger<NetMQMessageBrokerServer> logger)
    {
        this.options = options;
        this.logger = logger;

        publisherSocker = new XPublisherSocket($"@tcp://{options.Value.AddressForSubscribers}:{options.Value.PortForSubscribers}");
        subscriberSocket = new XSubscriberSocket($"@tcp://{options.Value.AddressForPublishers}:{options.Value.PortForPublishers}");
        consumerRouterSocket = new RouterSocket($"@tcp://{options.Value.AddressForProducers}:{options.Value.PortForProducers - 100}");
        producerRouterSocket = new RouterSocket($"@tcp://{options.Value.AddressForProducers}:{options.Value.PortForProducers}");
        producerRouterSocket.ReceiveReady += (s, a) =>
        {
            var clientMessage = a.Socket.ReceiveMultipartMessage(3);
            logger.LogDebug("Message received from producer");
            if (clientMessage.FrameCount != 3)
                logger.LogInformation($"Request recieved with unexpected format");

            var clientAddress = clientMessage[0];
            var clientAddressString = clientAddress.ConvertToString();

            NetMQFrame? workerAddress = null;
            if(clientAddressString.Contains("1"))
            {
                workerAddress = new NetMQFrame("client2");
            }
            else
            {
                workerAddress = new NetMQFrame("client1");
            }
            DeserializedMessageResult deserializedMessageResult = MessageSerializer.DeserializeMessage(clientMessage[2].Buffer);

            if(deserializedMessageResult.Message is CheckInMessage checkIn)
            {
                logger.LogDebug("CheckIn message received");
                return;
            }

            logger.LogInformation($"Request recieved from {clientAddress.ConvertToString()}");         
           // string response = "response";
            var messageToClient = new NetMQMessage();
            messageToClient.Append(workerAddress);
            messageToClient.AppendEmptyFrame();
            messageToClient.Append(clientAddress);            
            messageToClient.AppendEmptyFrame();
            messageToClient.Append(clientMessage[2].Buffer);

            logger.LogDebug("Sending response to producer");
            //            a.Socket.SendMultipartMessage(messageToClient);
            consumerRouterSocket.SendMultipartMessage(messageToClient);
            logger.LogDebug($"Response sent to producer {workerAddress.ConvertToString()}");
        };

        poller.Add(producerRouterSocket);
      
        //poller.Add(producerRouterSocket);


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
            logger.LogDebug("NetMQ proxy stopped");
        }
    }
    public Task StartAsync()
    {
        return Task.Run(Start);
    }

    public void Dispose()
    {
        publisherSocker.Dispose();
        subscriberSocket.Dispose();
        producerRouterSocket.Dispose();
        consumerRouterSocket.Dispose();
        logger.LogInformation("NetMQ proxy disposed");
    }
}
