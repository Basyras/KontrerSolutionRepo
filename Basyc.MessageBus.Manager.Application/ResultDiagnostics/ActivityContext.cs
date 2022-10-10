using Basyc.Diagnostics.Shared.Durations;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public record class ActivityContext(ServiceIdentity Service, string TraceId, ActivityContext? ParentActivity, string Id, string DisplayName, DateTimeOffset StartTime)
	{
		public event EventHandler? ActivityEnded;
		public event EventHandler? NestedActivityAdded;
		public event EventHandler? NestedActivityEnded;

		private readonly List<ActivityContext> nestedActivities = new();
		public IReadOnlyList<ActivityContext> NestedActivities { get => nestedActivities; }
		public bool HasEnded { get; private set; }
		public DateTimeOffset EndTime { get; private set; }
		public ActivityStatusCode Status { get; private set; }
		public void End(DateTimeOffset endTime, ActivityStatusCode status)
		{
			EndTime = endTime;
			Status = status;
			HasEnded = true;
			ActivityEnded?.Invoke(this, EventArgs.Empty);
		}
		public void AddNestedActivity(ActivityContext activity)
		{
			nestedActivities.Add(activity);
			activity.ActivityEnded += NestedActivity_Ended;
			NestedActivityAdded?.Invoke(this, EventArgs.Empty);
		}

		private void NestedActivity_Ended(object? sender, EventArgs e)
		{
			NestedActivityEnded?.Invoke(this, EventArgs.Empty);
		}
	}
}
