namespace Basyc.MessageBus.Client.NetMQ
{
    public class TypedMessageHandlerManagerOptions
    {
        public List<TypedMessageHandlerInfo> Handlers { get; } = new List<TypedMessageHandlerInfo>();
    }
}