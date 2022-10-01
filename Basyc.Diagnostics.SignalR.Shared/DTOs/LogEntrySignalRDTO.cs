using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;

namespace Basyc.Diagnostics.SignalR.Shared.DTOs
{
	public record LogEntrySignalRDTO(string TraceId, DateTimeOffset Time, LogLevel LogLevel, string Message)
	{
		public static LogEntrySignalRDTO FromLogEntry(LogEntry logEntry)
		{
			return new LogEntrySignalRDTO(logEntry.TraceId, logEntry.Time, logEntry.LogLevel, logEntry.Message);
		}

		public static LogEntry ToLogEntry(LogEntrySignalRDTO logEntryDTO)
		{
			return new LogEntry(TraceId: logEntryDTO.TraceId, logEntryDTO.Time, logEntryDTO.LogLevel, logEntryDTO.Message);
		}
	}



}
