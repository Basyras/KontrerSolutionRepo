namespace Basyc.MessageBus.NetMQ.Shared
{
    public record EventCase(int SessionId, string EventType, object EventData);
}