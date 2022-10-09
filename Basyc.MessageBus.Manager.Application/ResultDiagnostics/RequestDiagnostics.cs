using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class RequestDiagnostics
	{
		private readonly object lockObject = new object();

		private readonly List<LogEntry> logEntries = new List<LogEntry>();
		public IReadOnlyList<LogEntry> LogEntries { get => logEntries; }

		private readonly Dictionary<string, Activity> activityIdToActivityMap = new();

		private readonly List<Activity> activities = new List<Activity>();
		public IReadOnlyList<Activity> Activities { get => activities; }


		public string TraceId { get; init; }
		public event EventHandler<LogEntry>? LogReceived;
		public event EventHandler<ActivityStart>? ActivityStartReceived;
		public event EventHandler<ActivityEnd>? ActivityEndReceived;


		public RequestDiagnostics(string traceId)
		{
			TraceId = traceId;
		}

		public void Log(ServiceIdentity service, LogLevel logLevel, string message)
		{
			Log(service, DateTimeOffset.UtcNow, logLevel, message);
		}

		public void Log(ServiceIdentity service, DateTimeOffset time, LogLevel logLevel, string message)
		{
			LogEntry newLogEntry = new(service, TraceId, time, logLevel, message);
			logEntries.Add(newLogEntry);
			OnLogAdded(newLogEntry);
		}

		public void Log(LogEntry newLogEntry)
		{
			if (newLogEntry.TraceId != TraceId)
				throw new ArgumentException("Request id does not match context reuqest result id", nameof(newLogEntry));

			logEntries.Add(newLogEntry);
			OnLogAdded(newLogEntry);
		}

		public void StartActivity(ActivityStart activityStart)
		{
			lock (lockObject)
			{
				Activity activity = new Activity(activityStart.Service, activityStart.TraceId, activityStart.ParentId, activityStart.Id, activityStart.Name, activityStart.StartTime);
				activities.Add(activity);
				activityIdToActivityMap.Add(activity.Id, activity);
				OnActivityStartReceived(activityStart);
			}

		}

		public void EndActivity(ActivityEnd activityEnd)
		{
			try
			{
				lock (lockObject)
				{
					var activity = activityIdToActivityMap[activityEnd.Id];
					activity.End(activityEnd.EndTime, activityEnd.Status);
					OnActivityEndReceived(activityEnd);
				}

			}
			catch (Exception ex)
			{

			}

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
