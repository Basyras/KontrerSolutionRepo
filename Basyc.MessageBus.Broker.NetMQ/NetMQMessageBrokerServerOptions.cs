namespace Basyc.MessageBus.Broker.NetMQ
{
    public class NetMQMessageBrokerServerOptions
    {
        public string AddressForPublishers { get; set; } = "localhost";
        public int PortForPublishers { get; set; } = 5551;

        public string AddressForSubscribers { get; set; } = "localhost";
        public int PortForSubscribers { get; set; } = 5552;

        public string AddressForProducers { get; set; } = "localhost";
        public int PortForProducers { get; set; } = 5553;
    }
}