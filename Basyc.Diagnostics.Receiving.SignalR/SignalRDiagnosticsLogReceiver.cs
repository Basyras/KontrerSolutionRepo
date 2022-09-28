using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.Diagnostics.SignalR.Shared;
using Basyc.Extensions.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Basyc.Diagnostics.Receiving.SignalR
{
	public class SignalRDiagnosticsLogReceiver : IDiagnosticsLogReceiver, IReceiversMethodsServerCanCall
	{
		private readonly IStrongTypedHubConnectionPusherAndReceiver<IServerMethodsReceiversCanCall, IReceiversMethodsServerCanCall> hubConnection;
		public event EventHandler<LogsReceivedArgs>? LogsReceived;

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

		public async Task StartReceiving()
		{
			await hubConnection.StartAsync();
		}

		public Task ReceiveLogEntriesFromServer(LogEntrySignalRDTO[] logEntriesDTOs)
		{
			LogEntry[] logEntries = logEntriesDTOs.Select(x => LogEntrySignalRDTO.ToLogEntry(x)).ToArray();
			OnLogsReceived(logEntries);
			return Task.CompletedTask;
		}
	}
}
