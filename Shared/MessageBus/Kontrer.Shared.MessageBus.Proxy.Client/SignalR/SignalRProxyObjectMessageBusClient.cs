using Basyc.Extensions.SignalR.Client;
using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Client.SignalR;
using Basyc.MessageBus.HttpProxy.Shared.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Client.Http
{
	public class SignalRProxyObjectMessageBusClient : IObjectMessageBusClient
	{
		private readonly IStrongTypedHubConnection<IProxyClientMethods> hubConnection;

		public SignalRProxyObjectMessageBusClient(IOptions<SignalROptions> options)
		{
			hubConnection = new HubConnectionBuilder()
			.WithUrl(options.Value.SignalRServerUri + options.Value.ProxyClientHubPattern)
			.WithAutomaticReconnect()
			.BuildStrongTyped<IProxyClientMethods, IProxyServerMethods>(new ProxyServerMethods());
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			return hubConnection.StartAsync();
		}

		public void Dispose()
		{
			hubConnection.DisposeAsync().GetAwaiter().GetResult();
		}

		public Task PublishAsync(string eventType, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task PublishAsync(string eventType, object eventData, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<object> RequestAsync(string requestType, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public BusTask<object> RequestAsync(string requestType, object requestData, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task SendAsync(string commandType, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task SendAsync(string commandType, object commandData, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

	}
}