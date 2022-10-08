using Basyc.Diagnostics.Shared.Logging;
using System;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class InMemoryRequestDiagnosticsSource : IRequestDiagnosticsSource
	{
		public event EventHandler<LogsUpdatedArgs>? LogsReceived;
		public event EventHandler<ActivityStartsReceivedArgs>? ActivityStartsReceived;
		public event EventHandler<ActivityEndsReceivedArgs>? ActivityEndsReceived;

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsUpdatedArgs(logEntries));
		}

		public void PushLog(LogEntry logEntry)
		{
			OnLogsReceived(new LogEntry[] { logEntry });
		}
	}
}
