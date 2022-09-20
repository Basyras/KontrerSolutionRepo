namespace Basyc.MessageBus.HttpProxy.Shared.SignalR
{
	public record ResponseSignalRDTO(int SessionId, bool HasResponse, byte[]? ResponseData = null, string? ResponseType = null);
}
