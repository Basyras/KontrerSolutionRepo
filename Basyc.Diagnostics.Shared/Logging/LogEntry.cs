using Microsoft.Extensions.Logging;

namespace Basyc.Diagnostics.Shared.Logging
{
	public record LogEntry(int RequestId, DateTimeOffset Time, LogLevel LogLevel, string Message);

}
