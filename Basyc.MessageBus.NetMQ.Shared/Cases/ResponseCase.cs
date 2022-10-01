namespace Basyc.MessageBus.NetMQ.Shared.Cases
{
	public record ResponseCase(int SessionId, string TraceId, byte[] ResponseBytes, string ResponseType)
		 : CaseBase(SessionId, TraceId);
}
