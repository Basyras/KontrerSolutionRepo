using Basyc.Diagnostics.Producing.Shared.Building;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class IServiceCollectionDiagnosticsProducingExtensions
	{
		public static SelectProducerStage AddBasycDiagnosticsProducing(this IServiceCollection services)
		{
			return new SelectProducerStage(services);
		}

	}
}
