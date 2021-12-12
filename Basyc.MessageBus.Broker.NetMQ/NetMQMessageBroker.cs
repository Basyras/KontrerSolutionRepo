using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Basyc.MessageBus.Broker.NetMQ;

//https://netmq.readthedocs.io/en/latest/xpub-xsub/
public class NetMQMessageBroker : IMessageBroker
{
    private readonly IOptions<NetMQMessageBrokerOptions> options;
    private readonly ILogger<NetMQMessageBroker> logger;
    private readonly XPublisherSocket publisherSocker;
    private readonly XSubscriberSocket subscriberSocket;

    public NetMQMessageBroker(IOptions<NetMQMessageBrokerOptions> options, ILogger<NetMQMessageBroker> logger)
    {
        this.options = options;
        this.logger = logger;
        publisherSocker = new XPublisherSocket($"@tcp://127.0.0.1:{options.Value.PortForSubscribers}");
        subscriberSocket = new XSubscriberSocket($"@tcp://127.0.0.1:{options.Value.PortForPublishers}");

    }

    public void Start()
    {
        try
        {
            Proxy proxy = new Proxy(subscriberSocket, publisherSocker);
            logger.LogInformation("NetMQ proxy starting");
            proxy.Start();
            logger.LogInformation("NetMQ proxy stopped");

        }
        catch (Exception ex)
        {
            logger.LogInformation("NetMQ proxy stopped");
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
        logger.LogInformation("NetMQ proxy disposed");
    }
}
