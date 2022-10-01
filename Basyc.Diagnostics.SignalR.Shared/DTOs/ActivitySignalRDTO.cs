using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Logging;
using System.Diagnostics;

namespace Basyc.Diagnostics.SignalR.Shared.DTOs
{
	public record ActivitySignalRDTO(string ServiceName, string OperatioName, string TraceId, DateTimeOffset StarTime, DateTimeOffset EndTime, ActivityStatusCode Status)
	{
		public static ActivitySignalRDTO FromEntry(ActivityEntry activity)
		{
			return new ActivitySignalRDTO(activity.Service.ServiceName, activity.Name, activity.TraceId, activity.StartTime, activity.EndTime, activity.Status);
		}

		public static ActivityEntry ToEntry(ActivitySignalRDTO activityDTO)
		{
			return new ActivityEntry(new(activityDTO.ServiceName), activityDTO.TraceId, activityDTO.OperatioName, activityDTO.StarTime, activityDTO.EndTime, activityDTO.Status);
		}
	}



}
