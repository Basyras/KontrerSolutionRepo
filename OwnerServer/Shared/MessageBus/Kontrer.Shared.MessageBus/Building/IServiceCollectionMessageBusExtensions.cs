using Basyc.MessageBus.Client.Building;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class IServiceCollectionMessageBusExtensions
	{
		public static BusClientSetupProxyStage AddBasycMessageBusClient(this IServiceCollection services)
		{
			return new BusClientSetupProxyStage(services);
		}
	}
}