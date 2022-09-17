using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class SignalRDiagnosticsServer : IDiagnosticsServer
	{
		private readonly IHubContext<LoggingReceiversHub, ILoggingReceiversMethods> receiversHubContext;

		public SignalRDiagnosticsServer(IHubContext<LoggingReceiversHub, ILoggingReceiversMethods> receiversHubContext)
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
