using Microsoft.Extensions.Logging;

namespace Basyc.Diagnostics.Producing.SignalR.Shared
{
	public record LogEntrySignalRDTO(int RequestId, DateTimeOffset Time, LogLevel LogLevel, string Message);
}
