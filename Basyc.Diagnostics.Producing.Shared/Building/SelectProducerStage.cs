using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Basyc.Diagnostics.Producing.Shared.Building
{
	public class SelectProducerStage : BuilderStageBase
	{
		public SelectProducerStage(IServiceCollection services) : base(services)
		{
		}

		public void SelectInMemoryProducer()
		{
			services.TryAddSingleton<InMemoryDiagnosticsProducer>();
			services.AddSingleton<IDiagnosticsProducer, InMemoryDiagnosticsProducer>(x => x.GetRequiredService<InMemoryDiagnosticsProducer>());
		}
	}
}
