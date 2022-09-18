using Basyc.Extensions.SignalR.Client;
using Castle.DynamicProxy;

namespace Microsoft.AspNetCore.SignalR.Client
{
	public static class HubConnectionBasycExtensions
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

		public static IStrongTypedHubConnection<TMethodsClientCanCall> CreateStrongTyped<TMethodsClientCanCall>(this HubConnection connection)
			where TMethodsClientCanCall : class
		{
			HubClientInteceptor hubClientInteceptor = new HubClientInteceptor(connection, typeof(TMethodsClientCanCall));
			var hubClientProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<TMethodsClientCanCall>(hubClientInteceptor);
			return new StrongTypedHubConnection<TMethodsClientCanCall>(hubClientProxy, connection);
		}

		public static IStrongTypedHubConnection CreateStrongTyped<TMethodsServerCanCall>(this HubConnection connection, TMethodsServerCanCall methodsServerCanCall)
		{
			return new StrongTypedHubConnectionReceiver<TMethodsServerCanCall>(connection, methodsServerCanCall);
		}

		public static IStrongTypedHubConnection<TMethodsClientCanCall> CreateStrongTyped<TMethodsClientCanCall, TMethodsServerCanCall>(this HubConnection connection, TMethodsServerCanCall methodsServerCanCall)
			where TMethodsClientCanCall : class
		{
			HubClientInteceptor hubClientInteceptor = new HubClientInteceptor(connection, typeof(TMethodsClientCanCall));
			var hubClientProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<TMethodsClientCanCall>(hubClientInteceptor);
			return new StrongTypedHubConnection<TMethodsClientCanCall, TMethodsServerCanCall>(hubClientProxy, connection, methodsServerCanCall);
		}

		public static HubConnection ForwardTo<TMethodsServerCanCall>(this HubConnection hubConnection, TMethodsServerCanCall methodsServerCanCall)
		{
			HubListener.ForwardTo(hubConnection, methodsServerCanCall);
			return hubConnection;
		}
	}
}