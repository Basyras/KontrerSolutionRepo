using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.Client.Diagnostics.Sinks;
using Basyc.MessageBus.Client.Diagnostics.Sinks.BasycDiagnostics;
using Basyc.MessageBus.Client.Diagnostics.Sinks.Http;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class LoggingExtensions
	{
		public static void SelectHttpExporter(this BusClientSetupDiagnosticsStage stage, string httpAddressToSendLogs)
		{
			ArgumentNullException.ThrowIfNull(httpAddressToSendLogs, nameof(httpAddressToSendLogs));

			stage.services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.Services.AddSingleton<IBusClientLogExporter, HttpBusClientLogExporter>();
				stage.services.Configure<HttpLogSinkOptions>(sinkOptions =>
				{
					sinkOptions.HttpAddressToSendLogs = httpAddressToSendLogs;
				});
				loggingBuilder.Services.AddHttpClient<IBusClientLogExporter, HttpBusClientLogExporter>();
			});
		}

		public static void SelectBasycDiagnosticsExporter(this BusClientSetupDiagnosticsStage stage)
		{
			stage.services.AddSingleton<IBusClientLogExporter, BasycDiagnosticsBusClientLogExporter>();
		}
	}
}
