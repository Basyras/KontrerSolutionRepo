using Basyc.Diagnostics.Producing.Shared;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class IServiceProviderDiagnosticsProductingExtensions
	{
		public static Task StartBasycDiagnosticsProducer(this IServiceProvider serviceProvider)
		{
			var producer = serviceProvider.GetRequiredService<IDiagnosticsProducer>();
			return producer.StartAsync();
		}
	}
}
