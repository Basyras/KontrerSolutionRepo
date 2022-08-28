using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Client.Building
{
	public class BusClientSetupProxyStage : BuilderStageBase
	{
		public BusClientSetupProxyStage(IServiceCollection services) : base(services)
		{
		}

		public BusClientSetupHandlersStage NoProxy()
		{
			return new BusClientSetupHandlersStage(services);
		}

	}
}
