using Basyc.Diagnostics.Producing.SignalR.Shared;

namespace Basyc.Diagnostics.SignalR.Shared
{
	public interface IServerMethodsProducersCanCall
	{
		Task ReceiveLogEntriesFromProducer(LogEntrySignalRDTO[] logEntryDTOs);
	}
}
