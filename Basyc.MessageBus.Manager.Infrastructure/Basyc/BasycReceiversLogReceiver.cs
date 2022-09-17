using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Infrastructure.Basyc
{
	public class BasycReceiversLogSource : ILogSource
	{
		private readonly ILogReceiver[] logReceivers;

		public BasycReceiversLogSource(IEnumerable<ILogReceiver> logReceivers)
		{
			this.logReceivers = logReceivers.ToArray();
			foreach (var logReceiver in logReceivers)
			{
				logReceiver.LogsReceived += LogReceiver_LogsReceived;
			}
		}

		public event EventHandler<LogsUpdatedArgs>? LogsReceived;

		private void LogReceiver_LogsReceived(object sender, LogsReceivedArgs e)
		{
			OnLogsReceived(e.LogEntries);
		}

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsUpdatedArgs(logEntries));
		}
	}
}
