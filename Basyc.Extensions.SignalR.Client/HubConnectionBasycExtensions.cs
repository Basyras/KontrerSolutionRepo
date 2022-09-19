using Basyc.Extensions.SignalR.Client;
using Castle.DynamicProxy;

namespace Microsoft.AspNetCore.SignalR.Client
{
	public static class HubConnectionBasycExtensions
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

		public static IStrongTypedHubConnectionPusher<TMethodsClientCanCall> CreateStrongTyped<TMethodsClientCanCall>(this HubConnection connection)
			where TMethodsClientCanCall : class
		{
			HubClientInteceptor hubClientInteceptor = new HubClientInteceptor(connection, typeof(TMethodsClientCanCall));
			var hubClientProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<TMethodsClientCanCall>(hubClientInteceptor);
			return new StrongTypedHubConnectionPusher<TMethodsClientCanCall>(hubClientProxy, connection);
		}

		public static IStrongTypedHubConnectionReceiver<TMethodsServerCanCall> CreateStrongTypedReceiver<TMethodsServerCanCall>(this HubConnection connection, TMethodsServerCanCall methodsServerCanCall)
		{
			return new StrongTypedHubConnectionReceiver<TMethodsServerCanCall>(connection, methodsServerCanCall);
		}

		public static IStrongTypedHubConnectionPusher<TMethodsClientCanCall> CreateStrongTyped<TMethodsClientCanCall, TMethodsServerCanCall>(this HubConnection connection, TMethodsServerCanCall methodsServerCanCall)
			where TMethodsClientCanCall : class
		{
			HubClientInteceptor hubClientInteceptor = new HubClientInteceptor(connection, typeof(TMethodsClientCanCall));
			var hubClientProxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<TMethodsClientCanCall>(hubClientInteceptor);
			return new StrongTypedHubConnectionPusherAndReceiver<TMethodsClientCanCall, TMethodsServerCanCall>(hubClientProxy, connection, methodsServerCanCall);
		}

		public static HubConnection OnMultiple<TMethodsServerCanCall>(this HubConnection hubConnection, TMethodsServerCanCall methodsServerCanCall)
		{
			OnMultipleExtension.OnMultiple(hubConnection, methodsServerCanCall);
			return hubConnection;
		}
	}
}