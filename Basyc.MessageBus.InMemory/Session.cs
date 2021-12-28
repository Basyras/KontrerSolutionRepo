namespace Basyc.MessageBus.Client.NetMQ;

public partial class NetMQMessageBusClient
{
    public record Session(int SessionId, TaskCompletionSource<object> ResponseSource);
}
