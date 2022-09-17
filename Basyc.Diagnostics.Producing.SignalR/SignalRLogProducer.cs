using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Receiving.SignalR;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Basyc.Diagnostics.Producing.SignalR
{

	public class SignalRLogProducer : ILogProducer
	{
		private bool isStarted = false;
		private bool isStarting = false;
		private bool isFailed = false;
		private readonly TaskCompletionSource connectionStartingSource = new TaskCompletionSource();


		private readonly HubConnection hubConnection;
		private readonly IOptions<SignalRLogReceiverOptions> options;

		public SignalRLogProducer(IOptions<SignalRLogReceiverOptions> options)
		{
			hubConnection = new HubConnectionBuilder()
				.WithUrl(options.Value.SignalRServerUri!)
				.Build();
			this.options = options;
		}

		public async Task ProduceLog(LogEntry logEntry)
		{
			if (await EnsureConnectionStarted() is false)
			{
				return;
			}
			await hubConnection.SendAsync(SignalRConstants.ReceiveLogEntriesFromProducerMessage, new LogEntrySignalRDTO[] { LogEntrySignalRDTO.FromLogEntry(logEntry) });
		}

		/// <summary>
		/// Returns false when failed to connect
		/// </summary>
		/// <returns></returns>
		public async Task<bool> StartAsync()
		{
			isStarting = true;
			try
			{
				await hubConnection.StartAsync();
			}
			catch (Exception ex)
			{
				isFailed = true;
				isStarting = false;
				connectionStartingSource.SetResult();
				return false;
			}
			isStarted = true;
			isStarting = false;
			connectionStartingSource.SetResult();
			return true;
		}

		/// <summary>
		/// Returns false when connection failed to start
		/// </summary>
		/// <returns></returns>
		private async Task<bool> EnsureConnectionStarted()
		{
			if (isStarting)
			{
				await connectionStartingSource.Task;
			}
			else
			{
				if (isStarted is false)
				{
					var didConnect = await StartAsync();
					if (didConnect is false)
					{
						return false;
					}
				}
			}

			if (isFailed)
			{
				return false;
			}

			return true;
		}
	}
}
