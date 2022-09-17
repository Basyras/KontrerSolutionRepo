using Microsoft.Extensions.DependencyInjection;

namespace Basyc.Diagnostics.Server.Abstractions.Building
{
	public static class IServiceCollectionDiagnosicsServerExtensions
	{
		public static SelectDiagnosticsServerStage AddBasycDiagnosticsServer(this IServiceCollection services)
		{
			return new SelectDiagnosticsServerStage(services);
		}
	}
}
