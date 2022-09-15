using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Producing.Shared.Building;
using Basyc.Diagnostics.Producing.SignalR;

namespace Microsoft.Extensions.DependencyInjection;

public static class SetupSignalRLogProducerStageExtensions
{
	public static SetupProducerStage AddSignalRProducer(this SelectProducerStage parent)
	{
		parent.services.AddSingleton<ILogProducer, SignalRLogProducer>();
		return new SetupProducerStage(parent.services);
	}
}
