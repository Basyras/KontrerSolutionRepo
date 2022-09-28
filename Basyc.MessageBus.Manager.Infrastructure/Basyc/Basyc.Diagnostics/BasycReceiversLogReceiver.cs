using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Basyc.MessageBus.Manager.Infrastructure.Building;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Infrastructure.Basyc.Basyc.Diagnostics
{
	public class BasycReceiversLogSource : ILogSource
	{
		private readonly IDiagnosticsLogReceiver[] logReceivers;
		private readonly IBasycDiagnosticsReceiverSessionMapper sessionMapper;

		public BasycReceiversLogSource(IEnumerable<IDiagnosticsLogReceiver> logReceivers, IBasycDiagnosticsReceiverSessionMapper sessionMapper)
		{
			this.logReceivers = logReceivers.ToArray();
			foreach (var logReceiver in logReceivers)
			{
				logReceiver.LogsReceived += LogReceiver_LogsReceived;
			}

			this.sessionMapper = sessionMapper;
		}

		public event EventHandler<LogsUpdatedArgs> LogsReceived;

		private void LogReceiver_LogsReceived(object sender, LogsReceivedArgs e)
		{
			var mappedSessions = e.LogEntries.Select(x => new LogEntry(sessionMapper.GetSessionId(x.SessionId), x.Time, x.LogLevel, x.Message)).ToArray();
			OnLogsReceived(mappedSessions);
		}

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsUpdatedArgs(logEntries));
		}
	}
}
