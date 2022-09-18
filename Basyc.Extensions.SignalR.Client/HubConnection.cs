using Microsoft.AspNetCore.SignalR.Client;

namespace Basyc.Extensions.SignalR.Client
{
	internal class HubConnection<THubClient> : IHubConnection<THubClient>, IAsyncDisposable
	{
		public HubConnection(THubClient hubClient, HubConnection hubConnection)
		{
			Methods = hubClient;
			UnderlyingHubConnection = hubConnection;
		}

		public THubClient Methods { get; }
		public HubConnection UnderlyingHubConnection { get; }

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return UnderlyingHubConnection.StartAsync(cancellationToken);
		}

		public ValueTask DisposeAsync()
		{
			return UnderlyingHubConnection.DisposeAsync();
		}
	}
}
