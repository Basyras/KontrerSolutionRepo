using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.MessageBus.Manager.Infrastructure.SignalR.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.MessageBus.Manager.Infrastructure.SignalR.ResultDiagnostics
{
	public class LogSourceHub : Hub<ILogSourceClientMethods>
	{
		private readonly InMemoryLogSource inMemoryLogSource;

		public LogSourceHub(InMemoryLogSource inMemoryLogSource)
		{
			this.inMemoryLogSource = inMemoryLogSource;
		}

		public void SendLog(LogEntrySignalRDTO logEntryDTO)
		{
			inMemoryLogSource.PushLog(new LogEntry(logEntryDTO.RequestId, logEntryDTO.Time, logEntryDTO.LogLevel, logEntryDTO.Message));
		}
	}
}
