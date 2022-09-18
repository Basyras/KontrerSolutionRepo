using Microsoft.AspNetCore.SignalR.Client;

namespace Basyc.Extensions.SignalR.Client
{
	public interface IHubConnection<THubClient>
	{
		/// <summary>
		/// Property containing all strongly typed messages available to call.
		/// </summary>
		THubClient Methods { get; }
		HubConnection UnderlyingHubConnection { get; }

		Task StartAsync(CancellationToken cancellationToken = default);
	}
}
