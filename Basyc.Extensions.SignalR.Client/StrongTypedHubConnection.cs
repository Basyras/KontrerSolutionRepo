using Microsoft.AspNetCore.SignalR.Client;

namespace Basyc.Extensions.SignalR.Client
{

	internal abstract class StrongTypedHubConnectionBase : IStrongTypedHubConnection, IAsyncDisposable
	{
		public HubConnection UnderlyingHubConnection { get; }

		public StrongTypedHubConnectionBase(HubConnection hubConnection)
		{
			UnderlyingHubConnection = hubConnection;
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			return UnderlyingHubConnection.StartAsync(cancellationToken);
		}

		public ValueTask DisposeAsync()
		{
			return UnderlyingHubConnection.DisposeAsync();
		}
	}

	internal class StrongTypedHubConnection<TMethodsClientCanCall> : StrongTypedHubConnectionBase, IStrongTypedHubConnection<TMethodsClientCanCall>
	{
		public TMethodsClientCanCall Call { get; }

		public StrongTypedHubConnection(TMethodsClientCanCall clientMethods, HubConnection hubConnection) : base(hubConnection)
		{
			Call = clientMethods;
		}
	}

	internal class StrongTypedHubConnectionReceiver<TMethodsServerCanCall> : StrongTypedHubConnectionBase
	{
		public StrongTypedHubConnectionReceiver(HubConnection hubConnection, TMethodsServerCanCall serverMethods) : base(hubConnection)
		{
			HubListener.ForwardTo<TMethodsServerCanCall>(hubConnection, serverMethods);
		}
	}

	internal class StrongTypedHubConnection<TMethodsClientCanCall, TMethodsServerCanCall> : StrongTypedHubConnection<TMethodsClientCanCall>
	{
		public StrongTypedHubConnection(TMethodsClientCanCall clientMethods, HubConnection hubConnection, TMethodsServerCanCall serverMethods) : base(clientMethods, hubConnection)
		{
			HubListener.ForwardTo<TMethodsServerCanCall>(hubConnection, serverMethods);
		}

	}


}
