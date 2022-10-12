using Basyc.DependencyInjection;
using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Shared.Durations;
using Basyc.MessageBus.Client.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
			services.TryAddSingleton<IDiagnosticsProducer, NullDiagnosticsProducer>();
			services.Configure((System.Action<UseDiagnosticsOptions>)(x =>
			{
				x.UseDiagnostics = false;
			}));
		}

		public BusClientSetupDiagnosticsStage UseDiagnostics(string serviceName)
		{
			ServiceIdentity serviceIdentity = new ServiceIdentity(serviceName);

			services.Configure((System.Action<UseDiagnosticsOptions>)(x =>
			{
				x.UseDiagnostics = true;
				x.Service = serviceIdentity;
			}));
			IDiagnosticsProducer.ApplicationWideServiceIdentity = serviceIdentity;
			return new BusClientSetupDiagnosticsStage(services);
		}


	}
}
