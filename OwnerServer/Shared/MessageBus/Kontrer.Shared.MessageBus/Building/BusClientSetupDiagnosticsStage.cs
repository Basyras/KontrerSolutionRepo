using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Client.Building
{
	public class BusClientSetupDiagnosticsStage : BuilderStageBase
	{
		public BusClientSetupDiagnosticsStage(IServiceCollection services) : base(services)
		{
		}

#pragma warning disable CA1822 // Mark members as static
		public void NoDiagnostics()
#pragma warning restore CA1822 // Mark members as static
		{
		}
	}
}
