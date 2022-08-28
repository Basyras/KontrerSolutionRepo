using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class LoggingExtensions
	{
		public static void AddDiagnostics(this IServiceCollection services)
		{
			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.Services.AddSingleton<IBusHandlerTemporaryLogStorage>();
				loggingBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, BusHandlerLoggerProvider>());
			});
		}

	}
}
