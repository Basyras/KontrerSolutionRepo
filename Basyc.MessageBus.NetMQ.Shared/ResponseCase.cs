namespace Basyc.MessageBus.NetMQ.Shared
{
	public record ResponseCase(int SessionId, byte[] ResponseBytes, string ResponseType);
}
