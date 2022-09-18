using Basyc.MessageBus.HttpProxy.Server.Asp.Building;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class SelectProxyStageSignalRExtensions
	{
		public static void UseSignalR(this SelectProxyStage parent)
		{
			parent.services.AddSignalR();
		}
	}
}