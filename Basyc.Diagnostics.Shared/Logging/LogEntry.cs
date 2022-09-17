using Microsoft.Extensions.Logging;

namespace Basyc.Diagnostics.Shared.Logging
{
	public record LogEntry(int SessionId, DateTimeOffset Time, LogLevel LogLevel, string Message);

}
