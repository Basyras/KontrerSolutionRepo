using Microsoft.AspNetCore.SignalR.Client;

namespace Basyc.Extensions.SignalR.Client
{
	public interface IStrongTypedHubConnection<TMethodsClientCanCall> : IStrongTypedHubConnection
	{
		/// <summary>
		/// Property containing all strongly typed messages available to call from to server.
		/// </summary>
		TMethodsClientCanCall Call { get; }
	}

	public interface IStrongTypedHubConnection : IAsyncDisposable
	{
		/// <summary>
		/// Property containing all strongly typed messages available to call.
		/// </summary>
		HubConnection UnderlyingHubConnection { get; }
		Task StartAsync(CancellationToken cancellationToken = default);
	}
}
