using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class InMemoryRequestDiagnosticsSource : IRequestDiagnosticsSource
	{
		public event EventHandler<LogsUpdatedArgs>? LogsReceived;
		public event EventHandler<ActivitesUpdatedArgs>? ActivitiesReceived;

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsUpdatedArgs(logEntries));
		}

		public void PushLog(ServiceIdentity service, string traceId, LogLevel logLevel, string message)
		{
			OnLogsReceived(new LogEntry[] { new LogEntry(service, traceId, DateTimeOffset.UtcNow, logLevel, message) });
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
