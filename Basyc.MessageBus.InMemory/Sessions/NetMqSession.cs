namespace Basyc.MessageBus.Client.NetMQ.Sessions;

public record NetMqSession<TSessionResult>(int SessionId, string TraceId, string MessageType, TaskCompletionSource<TSessionResult> ResponseSource)
{
	public const int UnknownSessionId = -1;
}

