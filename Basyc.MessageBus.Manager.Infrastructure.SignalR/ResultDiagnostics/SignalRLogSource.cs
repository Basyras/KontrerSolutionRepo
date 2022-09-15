using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.MessageBus.Manager.Infrastructure.SignalR.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Basyc.MessageBus.Manager.Infrastructure.SignalR.ResultDiagnostics
{

	public class SignalRLogSource : ILogReceiver
	{
		private readonly HubConnection? hubConnection;
		private readonly IOptions<SignalRLogSourceOptions> options;

		public SignalRLogSource(IOptions<SignalRLogSourceOptions> options)
		{

			hubConnection = new HubConnectionBuilder()
				.WithUrl(options.Value.SignalRServerUri!)
				.Build();
			this.options = options;

			hubConnection.On<LogEntrySignalRDTO>(Constants.ReceiveLogEntryMessage, (LogEntryDTO) =>
			{
				var logEntry = new LogEntry(LogEntryDTO.RequestId, LogEntryDTO.Time, LogEntryDTO.LogLevel, LogEntryDTO.Message);
				OnLogReceived(logEntry);
			});
		}

		public event EventHandler<LogsReceivedArgs>? LogsReceived;
		private void OnLogReceived(LogEntry[] LogEntries)
		{
			LogsReceived?.Invoke(this, new LogsReceivedArgs(LogEntries));
		}

		private void OnLogReceived(LogEntry LogEntry)
		{
			OnLogReceived(new LogEntry[] { LogEntry });
		}
	}
}
