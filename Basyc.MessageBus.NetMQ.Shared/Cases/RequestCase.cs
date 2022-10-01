namespace Basyc.MessageBus.NetMQ.Shared.Cases
{
	public record RequestCase(int SessionId, string TraceId, string RequestType, byte[] RequestBytes, bool ExpectsResponse)
		 : CaseBase(SessionId, TraceId);
}
