﻿using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Basyc.Diagnostics.Producing.Shared.Building
{
	public class SetupProducersStage : BuilderStageBase
	{
		public SetupProducersStage(IServiceCollection services) : base(services)
		{
		}

		public SetupProducersStage SelectInMemoryProducer()
		{
			services.TryAddSingleton<InMemoryDiagnosticsProducer>();
			services.AddSingleton<IDiagnosticsExporter, InMemoryDiagnosticsProducer>(x => x.GetRequiredService<InMemoryDiagnosticsProducer>());
			return new SetupProducersStage(services);
		}
	}
}
