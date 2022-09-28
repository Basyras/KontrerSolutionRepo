using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Producing.SignalR.Shared;
using Basyc.Diagnostics.Receiving.SignalR;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.Diagnostics.SignalR.Shared;
using Basyc.Extensions.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Basyc.Diagnostics.Producing.SignalR
{

	public class SignalRLogProducer : ILogProducer
	{
		private bool isStarted = false;
		private bool isStarting = false;
		private bool isFailed = false;
		private readonly TaskCompletionSource connectionStartingSource = new();


		private readonly IStrongTypedHubConnectionPusher<IServerMethodsProducersCanCall> hubConnection;

		public SignalRLogProducer(IOptions<SignalRLogReceiverOptions> options)
		{
			hubConnection = new HubConnectionBuilder()
				.WithUrl(options.Value.SignalRServerUri!)
				.BuildStrongTyped<IServerMethodsProducersCanCall>();
		}

		public async Task ProduceLog(LogEntry logEntry)
		{
			if (await EnsureConnectionStarted() is false)
			{
				return;
			}
			await hubConnection.Call.ReceiveLogEntriesFromProducer(new LogEntrySignalRDTO[] { LogEntrySignalRDTO.FromLogEntry(logEntry) });
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
				Debug.WriteLine(ex.Message);
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
