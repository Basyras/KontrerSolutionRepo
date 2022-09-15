using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Receiving.Abstractions.Building;
using Basyc.Diagnostics.Receiving.SignalR.Building;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class SetupSignalRLogSourceStageExtensions
{
	public static SetupUriStage AddSignalRLogSource(this SelectReceiverProviderStage parent)
	{
		parent.services.TryAddSingleton<ILogSource, InMemoryLogSource>();
		return new SetupUriStage(parent.services);
	}
}
