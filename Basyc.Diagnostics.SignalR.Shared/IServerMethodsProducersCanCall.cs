using Basyc.Diagnostics.SignalR.Shared.DTOs;

namespace Basyc.Diagnostics.SignalR.Shared
{
	public interface IServerMethodsProducersCanCall
	{
		Task ReceiveLogsFromProducer(LogEntrySignalRDTO[] logEntryDTOs);
		Task ReceiveActivitiesFromProducer(ActivitySignalRDTO[] activityDTOs);
	}
}
