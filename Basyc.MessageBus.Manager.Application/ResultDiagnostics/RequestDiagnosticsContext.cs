using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class RequestDiagnosticsContext
	{
		private readonly DurationMapBuilder activityMapBuilder;
		private readonly List<LogEntry> logEntries = new List<LogEntry>();
		public IReadOnlyList<LogEntry> LogEntries { get => logEntries; }
		private readonly List<ActivityEntry> activities = new List<ActivityEntry>();
		public IReadOnlyList<ActivityEntry> Activities { get => activities; }


		public RequestResult RequestResult { get; init; }

		public RequestDiagnosticsContext(RequestResult requestResult, DurationMapBuilder activityMapBuilder)
		{
			RequestResult = requestResult;
			this.activityMapBuilder = activityMapBuilder;
		}

		public void AddLog(ServiceIdentity service, LogLevel logLevel, string message)
		{
			AddLog(service, DateTimeOffset.UtcNow, logLevel, message);
		}

		public void AddLog(ServiceIdentity service, DateTimeOffset time, LogLevel logLevel, string message)
		{
			LogEntry newLogEntry = new(service, RequestResult.TraceId, time, logLevel, message);
			logEntries.Add(newLogEntry);
			OnLogAdded(newLogEntry);
		}

		public void AddLog(LogEntry newLogEntry)
		{
			if (newLogEntry.TraceId != RequestResult.TraceId)
				throw new ArgumentException("Request id does not match context reuqest result id", nameof(newLogEntry));

			logEntries.Add(newLogEntry);
			OnLogAdded(newLogEntry);
		}

		public void AddActivity(ActivityEntry activity)
		{
			activities.Add(activity);
			var activitySegment = activityMapBuilder.StartNewSegment(activity.Service, activity.Name, activity.StartTime);
			activitySegment.End(activity.EndTime);
		}

		public event EventHandler<LogEntry>? LogAdded;
		private void OnLogAdded(LogEntry newLogEntry)
		{
			LogAdded?.Invoke(this, newLogEntry);
		}
	}
}
