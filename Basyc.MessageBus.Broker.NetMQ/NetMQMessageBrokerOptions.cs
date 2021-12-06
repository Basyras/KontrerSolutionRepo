namespace Basyc.MessageBus.Broker.NetMQ
{
    public class NetMQMessageBrokerOptions
    {
        public int PortForPublishers { get; set; } = 5551;
        public int PortForSubscribers { get; set; } = 5552;
    }
}