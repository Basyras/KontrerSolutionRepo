using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.SignalR.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class LoggingProducersHub : Hub<IProducersMethodsServerCanCall>, IServerMethodsProducersCanCall
	{
		private readonly IDiagnosticsServer diagnosticsServer;
		public LoggingProducersHub(IDiagnosticsServer diagnosticsServer)
		{
			this.diagnosticsServer = diagnosticsServer;
		}

		/// <summary>
		/// Name should be same as <see cref="SignalRConstants.ReceiveLogEntriesFromProducerMessage"/> value
		/// </summary>
		/// <param name="logEntryDTO"></param>
		public Task ReceiveLogEntriesFromProducer(LogEntrySignalRDTO[] logEntryDTOs)
		{
			var logEntries = logEntryDTOs.Select(dto => LogEntrySignalRDTO.ToLogEntry(dto)).ToArray();
			diagnosticsServer.ReceiveLogs(logEntries);
			return Task.CompletedTask;
		}



	}
}
