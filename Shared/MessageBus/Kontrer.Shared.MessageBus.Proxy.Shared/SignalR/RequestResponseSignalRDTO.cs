namespace Basyc.MessageBus.HttpProxy.Shared.SignalR
{
	public record RequestResponseSignalRDTO(string MessageType, bool HasResponse, byte[] ResponseData = null, string ResponseType = null);
}
