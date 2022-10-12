using Basyc.Diagnostics.Producing.Shared.Building;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class IServiceCollectionDiagnosticsProducingExtensions
	{
		public static SelectProducerStage AddBasycDiagnosticsProducer(this IServiceCollection services)
		{
			return new SelectProducerStage(services);
		}

	}
}
