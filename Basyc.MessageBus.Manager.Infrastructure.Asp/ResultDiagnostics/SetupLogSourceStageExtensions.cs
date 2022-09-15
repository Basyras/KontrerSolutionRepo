using Basyc.MessageBus.Manager.Infrastructure.Asp.ResultDiagnostics;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class SetupLogSourceStageExtensions
	{
		public static void AddHttpEndpointLogSource(this SetupLogSourceStage parent)
		{
			parent.services.AddSingleton<HttpEndpointLogSource>();
			throw new NotImplementedException();
		}
	}
}
