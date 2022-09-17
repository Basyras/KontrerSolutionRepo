using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.Client.Diagnostics.Sinks;
using Basyc.MessageBus.Client.Diagnostics.Sinks.BasycDiagnostics;
using Basyc.MessageBus.Client.Diagnostics.Sinks.Http;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class LoggingExtensions
	{
		public static void SelectHttpDiagnostics(this BusClientSetupDiagnosticsStage stage, string httpAddressToSendLogs)
		{
			ArgumentNullException.ThrowIfNull(httpAddressToSendLogs, nameof(httpAddressToSendLogs));

			stage.services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.Services.AddSingleton<ILogSink, HttpLogSink>();
				stage.services.Configure<HttpLogSinkOptions>(sinkOptions =>
				{
					sinkOptions.HttpAddressToSendLogs = httpAddressToSendLogs;
				});
				loggingBuilder.Services.AddHttpClient<ILogSink, HttpLogSink>();
			});
		}

		public static void UseBasycDiagnosticsProducers(this BusClientSetupDiagnosticsStage stage)
		{
			stage.services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.Services.AddSingleton<ILogSink, BasycDiagnosticsLogSink>();
			});
		}
	}
}
