using Basyc.Diagnostics.Shared.Durations;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public record class Activity(ServiceIdentity Service, string TraceId, string? ParentId, string Id, string DisplayName, DateTimeOffset StartTime)
	{
		public event EventHandler ActivityEnded;
		public List<Activity> NestedActivities { get; init; } = new();
		public DateTimeOffset EndTime { get; private set; }
		public ActivityStatusCode Status { get; private set; }
		public void End(DateTimeOffset endTime, ActivityStatusCode status)
		{
			EndTime = endTime;
			Status = status;
			ActivityEnded?.Invoke(this, EventArgs.Empty);
		}
	}
}
