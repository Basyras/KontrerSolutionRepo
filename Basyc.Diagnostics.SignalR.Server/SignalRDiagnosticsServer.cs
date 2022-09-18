using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.Diagnostics.SignalR.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class SignalRDiagnosticsServer : IDiagnosticsServer
	{
		private readonly IHubContext<LoggingReceiversHub, IReceiversMethodsServerCanCall> receiversHubContext;

		public SignalRDiagnosticsServer(IHubContext<LoggingReceiversHub, IReceiversMethodsServerCanCall> receiversHubContext)
		{
			this.receiversHubContext = receiversHubContext;
		}

		public Task ReceiveLogEntriesFromProducers(LogEntry[] logEntries)
		{
			ReceiveLogs(logEntries);
			return Task.CompletedTask;
		}

		public Task ReceiveLogs(LogEntry[] logEntries)
		{
			var logDtos = logEntries.Select(x => LogEntrySignalRDTO.FromLogEntry(x)).ToArray();
			receiversHubContext.Clients.All.ReceiveLogEntriesFromServer(logDtos);
			return Task.CompletedTask;
		}
	}
}
