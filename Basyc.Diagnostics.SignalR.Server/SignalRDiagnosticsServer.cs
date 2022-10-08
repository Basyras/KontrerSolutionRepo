using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.Diagnostics.SignalR.Shared;
using Basyc.Diagnostics.SignalR.Shared.DTOs;
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

		public Task ReceiveLogs(LogEntry[] logEntries)
		{
			var logDtos = logEntries.Select(x => LogEntrySignalRDTO.FromLogEntry(x)).ToArray();
			receiversHubContext.Clients.All.ReceiveChangesFromServer(new ChangesSignalRDTO(logDtos, Array.Empty<ActivityStartSignalRDTO>(), Array.Empty<ActivitySignalRDTO>()));
			return Task.CompletedTask;
		}

		public Task ReceiveStartedActivities(ActivityStart[] startedActivity)
		{
			var activityDTOs = startedActivity.Select(x => ActivityStartSignalRDTO.FromEntry(x)).ToArray();
			receiversHubContext.Clients.All.ReceiveChangesFromServer(new ChangesSignalRDTO(Array.Empty<LogEntrySignalRDTO>(), activityDTOs, Array.Empty<ActivitySignalRDTO>()));
			return Task.CompletedTask;
		}

		public Task ReceiveEndedActivities(ActivityEnd[] activities)
		{
			var activityDTOs = activities.Select(x => ActivitySignalRDTO.FromEntry(x)).ToArray();
			receiversHubContext.Clients.All.ReceiveChangesFromServer(new ChangesSignalRDTO(Array.Empty<LogEntrySignalRDTO>(), Array.Empty<ActivityStartSignalRDTO>(), activityDTOs));
			return Task.CompletedTask;
		}




	}
}
