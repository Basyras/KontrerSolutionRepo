using Basyc.Diagnostics.Producing.SignalR.Shared;

namespace Basyc.Diagnostics.SignalR.Shared
{
	public interface IReceiversMethodsServerCanCall
	{
		Task ReceiveLogEntriesFromServer(LogEntrySignalRDTO[] logEntries);
	}
}
