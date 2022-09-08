using Basyc.MessageBus.Client.Building;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class IServiceCollectionMessageBusExtensions
	{
		public static BusClientSetupProxyStage AddBasycBus(this IServiceCollection services)
		{
			return new BusClientSetupProxyStage(services);
		}
	}
}