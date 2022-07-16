namespace Basyc.MessageBus.Client.NetMQ.Sessions;

public record Session<TSessionResult>(int SessionId, string MessageType, TaskCompletionSource<TSessionResult> ResponseSource)
{
    public const int UnknownSessionId = -1;
}

