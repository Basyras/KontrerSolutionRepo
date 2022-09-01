using Basyc.MessageBus.Client.Diagnostics;
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
				loggingBuilder.Services.AddSingleton<ITemporaryLogStorage>();
				loggingBuilder.Services.RemoveAll(typeof(ILogger<>));
				services.Add(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(BusHandlerLoggerT<>)));

				//loggingBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, BusHandlerLoggerProvider>());
			});
		}

	}
}
