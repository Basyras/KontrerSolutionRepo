using Basyc.Extensions.SignalR.Client;
using Castle.DynamicProxy;

namespace Microsoft.AspNetCore.SignalR.Client
{
	public static class HubConnectionBuilderBasycExtensions
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

		public static IStrongTypedHubConnection BuildStrongTypedReceiver<TMethodsServerCanCall>(this IHubConnectionBuilder hubConnectionBuilder, TMethodsServerCanCall serverMethods)
		{
			var connection = hubConnectionBuilder.Build();
			return new StrongTypedHubConnectionReceiver<TMethodsServerCanCall>(connection, serverMethods);
		}

		public static IStrongTypedHubConnection<TMethodsClientCanCall> BuildStrongTyped<TMethodsClientCanCall>(this IHubConnectionBuilder hubConnectionBuilder)
			where TMethodsClientCanCall : class
		{
			CreateMethodsClientCanCallProxy<TMethodsClientCanCall>(hubConnectionBuilder, out var connection, out var hubClientProxy);
			return new StrongTypedHubConnection<TMethodsClientCanCall>(hubClientProxy, connection);
		}

		public static IStrongTypedHubConnection<TMethodsClientCanCall> BuildStrongTyped<TMethodsClientCanCall, TMethodsServerCanCall>(this IHubConnectionBuilder hubConnectionBuilder, TMethodsServerCanCall serverMethods)
			where TMethodsClientCanCall : class
		{
			CreateMethodsClientCanCallProxy<TMethodsClientCanCall>(hubConnectionBuilder, out var connection, out var hubClientProxy);
			return new StrongTypedHubConnection<TMethodsClientCanCall, TMethodsServerCanCall>(hubClientProxy, connection, serverMethods);
		}


		private static void CreateMethodsClientCanCallProxy<TMethodsClientCanCall>(IHubConnectionBuilder hubConnectionBuilder, out HubConnection connection, out TMethodsClientCanCall hubClientProxy)
			where TMethodsClientCanCall : class
		{
			connection = hubConnectionBuilder.Build();
			HubClientInteceptor hubClientInteceptor = new HubClientInteceptor(connection, typeof(TMethodsClientCanCall));
			hubClientProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<TMethodsClientCanCall>(hubClientInteceptor);
		}
	}
}