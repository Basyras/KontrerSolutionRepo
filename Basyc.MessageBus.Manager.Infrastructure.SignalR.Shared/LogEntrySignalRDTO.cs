using Microsoft.Extensions.Logging;

namespace Basyc.MessageBus.Manager.Infrastructure.SignalR.Shared
{
	public record LogEntrySignalRDTO(int RequestId, DateTimeOffset Time, LogLevel LogLevel, string Message);
}
