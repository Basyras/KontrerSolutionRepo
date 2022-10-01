using Basyc.DependencyInjection;
using Basyc.Diagnostics.Shared.Durations;
using Basyc.MessageBus.Client.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Client.Building
{
	public class BusClientUseDiagnosticsStage : BuilderStageBase
	{
		public BusClientUseDiagnosticsStage(IServiceCollection services) : base(services)
		{
		}

#pragma warning disable CA1822 // Mark members as static
		public void NoDiagnostics()
#pragma warning restore CA1822 // Mark members as static
		{
		}

		public BusClientSetupDiagnosticsStage UseDiagnostics(string serviceName)
		{
			services.Configure<UseDiagnosticsOptions>(x =>
			{
				x.Service = new ServiceIdentity(serviceName);
			});

			return new BusClientSetupDiagnosticsStage(services);
		}


	}
}
