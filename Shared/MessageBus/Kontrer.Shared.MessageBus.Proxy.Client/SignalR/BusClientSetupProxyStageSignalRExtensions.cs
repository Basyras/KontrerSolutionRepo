using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.HttpProxy.Client.Http;
using Basyc.MessageBus.HttpProxy.Shared.SignalR;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class BusClientSetupProxyStageSignalRExtensions
	{
		public static void UseSignalR(this BusClientSetupProxyStage parent, string signalRServerUri, string hubPattern = SignalRConstants.ProxyClientHubPattern)
		{
			parent.services.Configure<SignalROptions>(options =>
			{
				options.SignalRServerUri = signalRServerUri;
				options.ProxyClientHubPattern = hubPattern;
			});
		}
	}
}