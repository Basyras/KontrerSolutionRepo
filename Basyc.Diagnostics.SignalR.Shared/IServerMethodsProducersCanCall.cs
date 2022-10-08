using Basyc.Diagnostics.SignalR.Shared.DTOs;

namespace Basyc.Diagnostics.SignalR.Shared
{
	public interface IServerMethodsProducersCanCall
	{
		Task ReceiveLogsFromProducer(LogEntrySignalRDTO[] logEntryDTOs);
		Task ReceiveStartedActivitiesFromProducer(ActivityStartSignalRDTO[] activityStartsDTOs);
		Task ReceiveEndedActivitiesFromProducer(ActivitySignalRDTO[] activityDTOs);
	}
}
