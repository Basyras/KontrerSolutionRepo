﻿using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Basyc.MessageBus.Client.Diagnostics.Sinks.BasycDiagnostics
{
	public class BasycDiagnosticsBusClientLogExporter : IBusClientLogExporter
	{
		private readonly IDiagnosticsProducer[] logProducers;
		private readonly ILogger<BasycDiagnosticsBusClientLogExporter> logger;
		private readonly IOptions<UseDiagnosticsOptions> options;

		public BasycDiagnosticsBusClientLogExporter(IEnumerable<IDiagnosticsProducer> logProducers, ILogger<BasycDiagnosticsBusClientLogExporter> logger, IOptions<UseDiagnosticsOptions> options)
		{
			this.logProducers = logProducers.ToArray();
			this.logger = logger;
			this.options = options;
			Activity.DefaultIdFormat = ActivityIdFormat.W3C;
			Activity.ForceDefaultIdFormat = true;
			var listener = new ActivityListener();
			listener.ShouldListenTo = activity =>
			{
				return activity == DiagnosticSources.HandlerStarted;
			};
			listener.Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
			{
				return ActivitySamplingResult.AllDataAndRecorded;
			};
			listener.ActivityStopped += activity =>
			{
				SendCompletedActivity(new ActivityEntry(options.Value.Service, activity.ParentId, activity.OperationName, activity.StartTimeUtc, activity.StartTimeUtc + activity.Duration, activity.Status));
			};
			ActivitySource.AddActivityListener(listener);

		}
		public void SendLog<TState>(string handlerDisplayName, LogLevel logLevel, string traceId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var formattedMessage = formatter.Invoke(state, exception);
			foreach (var producer in logProducers)
			{
				try
				{
					producer.ProduceLog(new LogEntry(traceId, DateTimeOffset.UtcNow, logLevel, formattedMessage));
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"LogProducer: {producer.GetType().Name} failed to produce log with error {ex.Message}");
				}
			}
		}

		public void SendCompletedActivity(ActivityEntry activity)
		{
			foreach (var producer in logProducers)
			{
				try
				{
					producer.ProduceActivityEnd(activity);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"LogProducer: {producer.GetType().Name} failed to produce activity with error {ex.Message}");
				}
			}
		}
	}
}
