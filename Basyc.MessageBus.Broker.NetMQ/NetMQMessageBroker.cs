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
        publisherSocker = new XPublisherSocket($"tcp://localhost:{options.Value.PortForSubscribers}");
        subscriberSocket = new XSubscriberSocket($"tcp://localhost:{options.Value.PortForPublishers}");

    }

    public void Start()
    {
        Proxy proxy = new Proxy(subscriberSocket, publisherSocker);
        proxy.Start();
    }

    public void Dispose()
    {
        publisherSocker.Dispose();
        subscriberSocket.Dispose();
    }
}
