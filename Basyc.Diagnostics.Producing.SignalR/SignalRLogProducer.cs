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
			await hubConnection.SendAsync(Constants.ReceiveLogEntryMessage, logEntry);
		}
	}
}
