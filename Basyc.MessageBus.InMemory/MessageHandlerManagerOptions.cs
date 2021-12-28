namespace Basyc.MessageBus.Client.NetMQ
{
    public class MessageHandlerManagerOptions
    {
        public List<MessageHandlerInfo> Handlers { get; } = new List<MessageHandlerInfo>();
    }
}