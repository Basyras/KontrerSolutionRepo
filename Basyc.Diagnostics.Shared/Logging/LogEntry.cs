using Microsoft.Extensions.Logging;

namespace Basyc.Diagnostics.Shared.Logging
{
	public record struct LogEntry(string TraceId, DateTimeOffset Time, LogLevel LogLevel, string Message);

}
