namespace Basyc.MessageBus.NetMQ.Shared
{
	public record RequestCase(int SessionId, string RequestType, byte[] RequestBytes, bool ExpectsResponse/*, Type? ResponseType*/);
}
