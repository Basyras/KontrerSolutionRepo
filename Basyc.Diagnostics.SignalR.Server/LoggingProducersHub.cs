using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.SignalR.Shared;
using Basyc.Diagnostics.SignalR.Shared.DTOs;
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
		public Task ReceiveLogsFromProducer(LogEntrySignalRDTO[] logEntryDTOs)
		{
			var logEntries = logEntryDTOs.Select(dto => LogEntrySignalRDTO.ToLogEntry(dto)).ToArray();
			diagnosticsServer.ReceiveLogs(logEntries);
			return Task.CompletedTask;
		}

		public Task ReceiveStartedActivitiesFromProducer(ActivityStartSignalRDTO[] activityStartsDTOs)
		{
			var activities = activityStartsDTOs.Select(dto => ActivityStartSignalRDTO.ToEntry(dto)).ToArray();
			diagnosticsServer.ReceiveStartedActivities(activities);
			return Task.CompletedTask;
		}

		public Task ReceiveEndedActivitiesFromProducer(ActivitySignalRDTO[] activityDTOs)
		{
			var activities = activityDTOs.Select(dto => ActivitySignalRDTO.ToEntry(dto)).ToArray();
			diagnosticsServer.ReceiveEndedActivities(activities);
			return Task.CompletedTask;
		}
	}
}
