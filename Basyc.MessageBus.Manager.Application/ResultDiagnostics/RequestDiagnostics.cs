using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class RequestDiagnostics
	{
		private readonly object lockObject = new object();

		private readonly List<LogEntry> logEntries = new List<LogEntry>();
		public IReadOnlyList<LogEntry> LogEntries { get => logEntries; }

		private readonly Dictionary<string, ActivityContext> activityIdToActivityMap = new();
		public List<ServiceIdentityContext> services = new List<ServiceIdentityContext>();
		public IReadOnlyList<ServiceIdentityContext> Services { get => services; }


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
			Log(newLogEntry);
		}

		public void Log(LogEntry newLogEntry)
		{
			if (newLogEntry.TraceId != TraceId)
				throw new ArgumentException("Request id does not match context reuqest result id", nameof(newLogEntry));

			logEntries.Add(newLogEntry);
			logEntries.Sort((x, y) => x.Time.CompareTo(y.Time));
			OnLogAdded(newLogEntry);
		}

		public ActivityContext StartActivity(ActivityStart activityStart)
		{
			ServiceIdentityContext serviceVM = EnsureServiceCreated(activityStart.Service);

			ActivityContext? parentActivity = null;
			if (activityStart.ParentId is not null)
			{
				try
				{
					parentActivity = activityIdToActivityMap[activityStart.ParentId];

				}
				catch (Exception ex)
				{

				}

			}
			ActivityContext newActivity = new ActivityContext(activityStart.Service, activityStart.TraceId, parentActivity, activityStart.Id, activityStart.Name, activityStart.StartTime);
			activityIdToActivityMap.Add(newActivity.Id, newActivity);
			if (parentActivity is null)
			{
				serviceVM.AddActivity(newActivity);
			}
			else
			{
				parentActivity.AddNestedActivity(newActivity);
			}
			OnActivityStartReceived(activityStart);
			return newActivity;

		}

		public void EndActivity(ActivityEnd activityEnd)
		{
			if (activityIdToActivityMap.TryGetValue(activityEnd.Id, out var activity) is false)
			{
				activity = StartActivity(new ActivityStart(activityEnd.Service, activityEnd.TraceId, activityEnd.ParentId, activityEnd.Id, activityEnd.Name, activityEnd.StartTime));
			}
			activity.End(activityEnd.EndTime, activityEnd.Status);
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

		private ServiceIdentityContext EnsureServiceCreated(ServiceIdentity serviceIdentity)
		{
			ServiceIdentityContext? serviceVM = Services.FirstOrDefault(x => x.ServiceIdentity == serviceIdentity);
			if (serviceVM == null)
			{
				serviceVM = new ServiceIdentityContext(serviceIdentity);
				services.Add(serviceVM);
			}

			return serviceVM;
		}
	}
}
