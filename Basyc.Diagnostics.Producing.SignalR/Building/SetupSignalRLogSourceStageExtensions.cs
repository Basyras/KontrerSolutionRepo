using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Producing.Shared.Building;
using Basyc.Diagnostics.Producing.SignalR;

namespace Microsoft.Extensions.DependencyInjection;

public static class SetupSignalRLogProducerStageExtensions
{
	public static SetupProducerStage UseSignalR(this SelectProducerStage parent)
	{
		parent.services.AddSingleton<IDiagnosticsProducer, SignalRDiagnosticsProducer>();
		return new SetupProducerStage(parent.services);
	}
}
