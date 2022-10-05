using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Basyc.MessageBus.Manager.Infrastructure.Building;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Infrastructure.Basyc.Basyc.Diagnostics
{
	public class BasycDiagnosticsReceiversRequestDiagnosticsSource : IRequestDiagnosticsSource
	{
		private readonly IBasycDiagnosticsReceiverTraceIdMapper sessionMapper;
		public BasycDiagnosticsReceiversRequestDiagnosticsSource(IEnumerable<IDiagnosticsLogReceiver> logReceivers, IBasycDiagnosticsReceiverTraceIdMapper sessionMapper)
		{
			foreach (var logReceiver in logReceivers)
			{
				logReceiver.LogsReceived += LogReceiver_LogsReceived;
				logReceiver.ActivitiesReceived += LogReceiver_ActivitiesReceived;
			}

			this.sessionMapper = sessionMapper;
		}


		public event EventHandler<ActivitesUpdatedArgs> ActivitiesReceived;

		private void OnActivitiesReceived(ActivityEntry[] activities)
		{
			ActivitiesReceived?.Invoke(this, new ActivitesUpdatedArgs(activities));
		}

		private void LogReceiver_ActivitiesReceived(object sender, ActivitiesReceivedArgs e)
		{
			OnActivitiesReceived(e.Activities);
		}

		public event EventHandler<LogsUpdatedArgs> LogsReceived;

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsUpdatedArgs(logEntries));
		}
		private void LogReceiver_LogsReceived(object sender, LogsReceivedArgs e)
		{
			var mappedSessions = e.LogEntries.Select(x => new LogEntry(x.Service, sessionMapper.GetTraceId(x.TraceId), x.Time, x.LogLevel, x.Message)).ToArray();
			OnLogsReceived(mappedSessions);
		}
	}
}
