using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.Diagnostics.SignalR.Shared;

namespace Basyc.Diagnostics.Receiving.SignalR
{
	public class ReceiversMethodsServerCanCall : IReceiversMethodsServerCanCall
	{
		private readonly Action<LogEntry[]> logsReceivedAction;

		public ReceiversMethodsServerCanCall(Action<LogEntry[]> logsReceivedAction)
		{
			this.logsReceivedAction = logsReceivedAction;
		}
		public Task ReceiveLogEntriesFromServer(LogEntrySignalRDTO[] logEntriesDTOs)
		{
			LogEntry[] logEntries = logEntriesDTOs.Select(x => LogEntrySignalRDTO.ToLogEntry(x)).ToArray();
			logsReceivedAction.Invoke(logEntries);
			return Task.CompletedTask;
		}
	}
}
