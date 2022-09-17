using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Basyc.Diagnostics.Receiving.SignalR
{
	public class SignalRLogReceiver : ILogReceiver
	{

		private readonly HubConnection hubConnection;
		private readonly IOptions<SignalRLogReceiverOptions> options;

		public event EventHandler<LogsReceivedArgs>? LogsReceived;

		public SignalRLogReceiver(IOptions<SignalRLogReceiverOptions> options)
		{
			this.options = options;
			hubConnection = new HubConnectionBuilder()
				.WithUrl(options.Value.SignalRServerReceiverHubUri!)
				.WithAutomaticReconnect()
				.Build();

			hubConnection.On(SignalRConstants.ReceiveLogEntriesFromServerMessage, (Action<LogEntrySignalRDTO[]>)(logEntriesDtos =>
			{
				LogEntry[] logEntries = logEntriesDtos.Select(x => LogEntrySignalRDTO.ToLogEntry(x)).ToArray();
				OnLogsReceived(logEntries);
			}));
		}

		private void OnLogsReceived(LogEntry[] logEntries)
		{
			LogsReceived?.Invoke(this, new LogsReceivedArgs(logEntries));
		}

		public async Task StartReceiving()
		{
			await hubConnection.StartAsync();
		}


	}
}
