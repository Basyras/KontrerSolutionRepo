using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.Diagnostics.Receiving.Abstractions.Building;
using Basyc.Diagnostics.Receiving.SignalR;
using Basyc.Diagnostics.Receiving.SignalR.Building;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class SelectReceiverProviderStageSignalRExtensions
	{
		public static SetupSignalRReceiverStage UseSignalR(this SelectReceiverProviderStage parent)
		{
			parent.services.AddSingleton<ILogReceiver, SignalRLogReceiver>();
			return new SetupSignalRReceiverStage(parent.services);
		}
	}
}
