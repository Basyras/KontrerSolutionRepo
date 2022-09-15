using Basyc.Diagnostics.Producing.SignalR;
using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.MessageBus.Manager.Infrastructure.SignalR.ResultDiagnostics
{
	public class LogHub : Hub<ILogSourceClientMethods>
	{
		private readonly InMemoryLogSource inMemoryLogSource;

		public LogHub(InMemoryLogSource inMemoryLogSource)
		{
			this.inMemoryLogSource = inMemoryLogSource;
		}

		// Name should be same as <see cref="Constants.ReceiveLogEntryMessage"/> value
		public void ReceiveLogEntry(LogEntrySignalRDTO logEntryDTO)
		{
			inMemoryLogSource.PushLog(new LogEntry(logEntryDTO.RequestId, logEntryDTO.Time, logEntryDTO.LogLevel, logEntryDTO.Message));
		}
	}
}
