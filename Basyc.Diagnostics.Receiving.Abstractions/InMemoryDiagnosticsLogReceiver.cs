using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;

namespace Basyc.Diagnostics.Receiving.Abstractions
{
	public class InMemoryDiagnosticsLogReceiver : IDiagnosticsLogReceiver
	{
		public event EventHandler<LogsReceivedArgs>? LogsReceived;
		public event EventHandler<ActivitiesReceivedArgs>? ActivitiesReceived;

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsReceivedArgs(logEntries));
		}

		private void OnActivitiesReceived(ActivityEntry[] activities)
		{
			ActivitiesReceived?.Invoke(this, new ActivitiesReceivedArgs(activities));
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

		public Task StartReceiving()
		{
			return Task.CompletedTask;
		}
	}
}
