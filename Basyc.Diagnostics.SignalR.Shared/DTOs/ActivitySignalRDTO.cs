using Basyc.Diagnostics.Shared.Logging;
using System.Diagnostics;

namespace Basyc.Diagnostics.SignalR.Shared.DTOs
{
	public record ActivitySignalRDTO(string ServiceName, string? ParentId, string Id, string TraceId, string OperatioName, DateTimeOffset StarTime, DateTimeOffset EndTime, ActivityStatusCode Status)
	{
		public static ActivitySignalRDTO FromEntry(ActivityEnd activity)
		{
			return new ActivitySignalRDTO(activity.Service.ServiceName, activity.ParentId, activity.Id, activity.TraceId, activity.Name, activity.StartTime, activity.EndTime, activity.Status);
		}

		public static ActivityEnd ToEntry(ActivitySignalRDTO activityDTO)
		{
			return new ActivityEnd(new(activityDTO.ServiceName), activityDTO.TraceId, activityDTO.ParentId, activityDTO.Id, activityDTO.OperatioName, activityDTO.StarTime, activityDTO.EndTime, activityDTO.Status);
		}
	}



}
