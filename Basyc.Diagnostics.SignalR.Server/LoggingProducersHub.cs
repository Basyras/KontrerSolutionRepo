using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Server.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class LoggingProducersHub : Hub<ILoggingProducersMethods>
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

	//public class LoggingProducersHub : Hub
	//{
	//	/// <summary>
	//	/// Name should be same as <see cref="SignalRConstants.ReceiveLogEntriesFromProducerMessage"/> value
	//	/// </summary>
	//	/// <param name="logEntryDTO"></param>
	//	public void ReceiveLogEntriesFromProducer(LogEntrySignalRDTO[] logEntryDTOs)
	//	{
	//		var logEntries = logEntryDTOs.Select(dto => LogEntrySignalRDTO.ToLogEntry(dto)).ToArray();
	//	}
	//}
}
