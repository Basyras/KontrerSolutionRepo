using Basyc.Diagnostics.Shared.Logging;
using System.Diagnostics;

namespace Basyc.Diagnostics.Producing.Shared
{
	public interface IDiagnosticsProducer
	{
		Task ProduceLog(LogEntry logEntry);
		Task StartActivity(ActivityStart activityStart);
		Task EndActivity(ActivityEnd activityEnd);
		Task<bool> StartAsync();

		public Task EndActivity(ActivityStart activity, DateTimeOffset endtime = default, ActivityStatusCode status = ActivityStatusCode.Ok)
		{
			if (endtime == default)
				endtime = DateTimeOffset.Now;
			return EndActivity(new ActivityEnd(activity.Service, activity.TraceId, activity.ParentId, activity.Id, activity.Name, activity.StartTime, endtime, status));
		}



	}
}