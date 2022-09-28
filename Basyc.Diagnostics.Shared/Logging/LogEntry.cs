using Microsoft.Extensions.Logging;

namespace Basyc.Diagnostics.Shared.Logging
{
	public record struct LogEntry(int SessionId, DateTimeOffset Time, LogLevel LogLevel, string Message);

}
