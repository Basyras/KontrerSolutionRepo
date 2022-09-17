using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;

namespace Basyc.Diagnostics.Producing.SignalR.Shared
{
	public record LogEntrySignalRDTO(int SessionId, DateTimeOffset Time, LogLevel LogLevel, string Message)
	{
		public static LogEntrySignalRDTO FromLogEntry(LogEntry logEntry)
		{
			return new LogEntrySignalRDTO(logEntry.SessionId, logEntry.Time, logEntry.LogLevel, logEntry.Message);
		}

		public static LogEntry ToLogEntry(LogEntrySignalRDTO logEntryDTO)
		{
			return new LogEntry(logEntryDTO.SessionId, logEntryDTO.Time, logEntryDTO.LogLevel, logEntryDTO.Message);
		}
	}



}
