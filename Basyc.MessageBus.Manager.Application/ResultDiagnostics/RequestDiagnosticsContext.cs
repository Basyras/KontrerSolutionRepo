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

		private readonly Dictionary<string, Activity> activityIdToActivityMap = new();

		private readonly List<Activity> activities = new List<Activity>();
		public IReadOnlyList<Activity> Activities { get => activities; }


		public RequestResultContext RequestResult { get; init; }
		public event EventHandler<LogEntry>? LogReceived;
		public event EventHandler<ActivityStart>? ActivityStartReceived;
		public event EventHandler<ActivityEnd>? ActivityEndReceived;


		public RequestDiagnosticsContext(RequestResultContext requestResult, DurationMapBuilder activityMapBuilder)
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

		public void StartActivity(ActivityStart activityStart)
		{
			Activity activity = new Activity(activityStart.Service, activityStart.TraceId, activityStart.ParentId, activityStart.Id, activityStart.Name, activityStart.StartTime);
			activities.Add(activity);
			activityIdToActivityMap.Add(activity.Id, activity);
			//var activitySegment = activityMapBuilder.StartNewSegment(activityStart.Service, activityStart.Name, activityStart.StartTime);
			OnActivityStartReceived(activityStart);
		}

		public void EndActivity(ActivityEnd activityEnd)
		{
			var activity = activityIdToActivityMap[activityEnd.Id];
			activity.End(activityEnd.EndTime, activityEnd.Status);
			//activitySegment.End(activityEnd.EndTime);
			OnActivityEndReceived(activityEnd);

		}

		private void OnLogAdded(LogEntry newLogEntry)
		{
			LogReceived?.Invoke(this, newLogEntry);
		}

		private void OnActivityStartReceived(ActivityStart activityStart)
		{
			ActivityStartReceived?.Invoke(this, activityStart);
		}

		private void OnActivityEndReceived(ActivityEnd activityEnd)
		{
			ActivityEndReceived?.Invoke(this, activityEnd);
		}
	}
}
