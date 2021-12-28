namespace Basyc.MessageBus.Client.NetMQ;

public record ActiveSession(int SessionId, TaskCompletionSource<object> ResponseSource)
{
    public const int UnknownSessionId = -1;
}

