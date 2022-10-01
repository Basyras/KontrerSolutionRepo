namespace Basyc.Diagnostics.SignalR.Shared.DTOs
{
	public record ChangesSignalRDTO(LogEntrySignalRDTO[] Logs, ActivitySignalRDTO[] Activities);
}
