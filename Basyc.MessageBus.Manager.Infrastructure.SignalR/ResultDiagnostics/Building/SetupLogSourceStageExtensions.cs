using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.Diagnostics;
using Basyc.MessageBus.Manager.Infrastructure.SignalR.ResultDiagnostics;
using Basyc.MessageBus.Manager.Infrastructure.SignalR.ResultDiagnostics.Building;

namespace Microsoft.Extensions.DependencyInjection;

public static class SetupLogSourceStageExtensions
{
	public static SetupUriStage AddSignalRLogSource(this SetupLogSourceStage parent)
	{
		parent.AddLogSource<SignalRLogSource>();
		return new SetupUriStage(parent.services);
	}

	//public static void AddSignalRLogSource(this SetupLogSourceStage parent)
	//{
	//	parent.services.TryAddSingleton<InMemoryLogSource>();
	//}
}
