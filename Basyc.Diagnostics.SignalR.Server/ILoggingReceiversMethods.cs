
using Basyc.Diagnostics.Producing.SignalR.Shared;

namespace Basyc.Diagnostics.SignalR.Server
{
	public interface ILoggingReceiversMethods
	{
		Task ReceiveLogEntriesFromServer(LogEntrySignalRDTO[] logEntries);
	}
}
