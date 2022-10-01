using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.Diagnostics.SignalR.Shared;
using Basyc.Diagnostics.SignalR.Shared.DTOs;
using Basyc.Extensions.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Basyc.Diagnostics.Receiving.SignalR
{
	public class SignalRDiagnosticsLogReceiver : IDiagnosticsLogReceiver, IReceiversMethodsServerCanCall
	{
		private readonly IStrongTypedHubConnectionPusherAndReceiver<IServerMethodsReceiversCanCall, IReceiversMethodsServerCanCall> hubConnection;
		public event EventHandler<LogsReceivedArgs>? LogsReceived;
		public event EventHandler<ActivitiesReceivedArgs>? ActivitiesReceived;

		public SignalRDiagnosticsLogReceiver(IOptions<SignalRLogReceiverOptions> options)
		{
			hubConnection = new HubConnectionBuilder()
				.WithUrl(options.Value.SignalRServerReceiverHubUri!)
				.WithAutomaticReconnect()
				.BuildStrongTyped<IServerMethodsReceiversCanCall, IReceiversMethodsServerCanCall>(this);
		}

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsReceivedArgs(logEntries));
		}

		private void OnActivitiesReceived(ActivityEntry[] activities)
		{
			ActivitiesReceived?.Invoke(this, new ActivitiesReceivedArgs(activities));
		}

		public async Task StartReceiving()
		{
			await hubConnection.StartAsync();
		}

		public Task ReceiveChangesFromServer(ChangesSignalRDTO changes)
		{
			if (changes.Logs.Any())
				receiveLogEntriesFromServer(changes.Logs);
			if (changes.Activities.Any())
				receiveActivitiesFromServer(changes.Activities);
			return Task.CompletedTask;
		}


		private void receiveLogEntriesFromServer(LogEntrySignalRDTO[] logEntriesDTOs)
		{
			var logEntries = logEntriesDTOs
				.Select(x => LogEntrySignalRDTO.ToLogEntry(x))
				.ToArray();
			OnLogsReceived(logEntries);
		}

		private void receiveActivitiesFromServer(ActivitySignalRDTO[] activitiesDTOs)
		{
			var activities = activitiesDTOs
				.Select(x => ActivitySignalRDTO.ToEntry(x))
				.ToArray();

			OnActivitiesReceived(activities);
		}
	}
}
