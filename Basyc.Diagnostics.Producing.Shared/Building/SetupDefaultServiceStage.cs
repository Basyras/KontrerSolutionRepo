using Basyc.DependencyInjection;
using Basyc.Diagnostics.Shared.Durations;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.Diagnostics.Producing.Shared.Building
{
	public class SetupDefaultServiceStage : BuilderStageBase
	{
		public SetupDefaultServiceStage(IServiceCollection services) : base(services)
		{
		}

		public SetupProducersStage SetDefaultService(string serviceName)
		{
			ServiceIdentity serviceIdentity = new ServiceIdentity(serviceName);
			IDiagnosticsExporter.ApplicationWideServiceIdentity = serviceIdentity;
			return new SetupProducersStage(services);
		}
	}
}
