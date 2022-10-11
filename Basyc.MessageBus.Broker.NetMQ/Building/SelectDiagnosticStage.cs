using Basyc.DependencyInjection;
using Basyc.Diagnostics.Producing.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Broker.NetMQ.Building
{
	public class SelectDiagnosticStage : BuilderStageBase
	{
		public SelectDiagnosticStage(IServiceCollection services) : base(services)
		{
		}

		public void SkipDiagnostics()
		{
			services.AddSingleton<IDiagnosticsProducer, NullDiagnosticsProducer>();
		}
	}
}
