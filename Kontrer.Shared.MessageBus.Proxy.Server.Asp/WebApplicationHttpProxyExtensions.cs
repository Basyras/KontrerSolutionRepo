using Basyc.MessageBus.HttpProxy.Server.Asp;

namespace Microsoft.AspNetCore.Builder
{
	public static class WebApplicationHttpProxyExtensions
	{
		public static WebApplication MapMessageBusProxy(this WebApplication app) => MapBusManagerProxy(app, "");
		public static WebApplication MapBusManagerProxy(this WebApplication app, string pattern)
		{
			app.MapPost(pattern, Constants.ProxyHandler);
			return app;
		}
	}
}
