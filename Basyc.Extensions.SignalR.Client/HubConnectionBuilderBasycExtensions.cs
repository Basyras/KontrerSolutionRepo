using Castle.DynamicProxy;
using Microsoft.AspNetCore.SignalR.Client;

namespace Basyc.Extensions.SignalR.Client
{
	public static class HubConnectionBuilderBasycExtensions
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();
		public static IHubConnection<THubClient> BuildStrongTyped<THubClient>(this IHubConnectionBuilder hubConnectionBuilder) where THubClient : class
		{
			HubConnection connection = hubConnectionBuilder.Build();
			HubClientInteceptor hubClientInteceptor = new HubClientInteceptor(connection, typeof(THubClient));
			var hubClientProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<THubClient>(hubClientInteceptor);
			return new HubConnection<THubClient>(hubClientProxy, connection);
		}

		public static IHubConnection<THubClient> CreateStrongTyped<THubClient>(this HubConnection connection) where THubClient : class
		{
			HubClientInteceptor hubClientInteceptor = new HubClientInteceptor(connection, typeof(THubClient));
			var hubClientProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<THubClient>(hubClientInteceptor);
			return new HubConnection<THubClient>(hubClientProxy, connection);
		}
	}
}