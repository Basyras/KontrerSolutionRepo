﻿using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.Client.Diagnostics;
using Basyc.MessageBus.Client.Diagnostics.Sinks;
using Basyc.MessageBus.Client.Diagnostics.Sinks.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
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
				loggingBuilder.Services.RemoveAll(typeof(ILogger<>));
				stage.services.Add(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(BusHandlerLoggerT<>)));
				//services.Add(ServiceDescriptor.Singleton(typeof(LoggerToBypassCircularDepedency<>)));
				stage.services.AddSingleton(typeof(LoggerToBypassCircularDepedency<>));
				stage.services.Configure<HttpLogSinkOptions>(sinkOptions =>
				{
					sinkOptions.HttpAddressToSendLogs = httpAddressToSendLogs;
				});
				loggingBuilder.Services.AddHttpClient<ILogSink, HttpLogSink>();
			});
		}

	}
}