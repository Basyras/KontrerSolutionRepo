using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class InMemoryLogSource : ILogSource
	{
		public event EventHandler<LogsUpdatedArgs>? LogsReceived;

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsUpdatedArgs(logEntries));
		}

		public void PushLog(int requestId, LogLevel logLevel, string message)
		{
			OnLogsReceived(new LogEntry[] { new LogEntry(requestId, DateTimeOffset.UtcNow, logLevel, message) });
		}

		public void PushLog(LogEntry logEntry)
		{
			OnLogsReceived(new LogEntry[] { logEntry });
		}

		public void PushLogs(params LogEntry[] logEntries)
		{
			OnLogsReceived(logEntries);
		}
	}
}
