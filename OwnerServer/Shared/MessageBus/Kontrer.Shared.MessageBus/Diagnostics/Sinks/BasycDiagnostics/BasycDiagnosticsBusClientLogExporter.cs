using Basyc.Diagnostics.Producing.Shared;
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
				return true;
			};
			listener.Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
			{
				return ActivitySamplingResult.AllDataAndRecorded;
			};
			listener.ActivityStarted += (Activity activity) =>
			{
				//DiagnosticConstants.HandlerStarted.
				if (activity.GetBaggageItem(DiagnosticConstants.ShouldBeReceived) != true.ToString())
				{
					if (activity.GetTagItem(DiagnosticConstants.ShouldBeReceived) as bool? != true)
						return;
				}

				if (activity.Context.IsRemote)
				{

				}
				string traceId = activity.TraceId.ToString().TrimStart('0');

				SendActivityStart(new ActivityStart(options.Value.Service, traceId, activity.ParentSpanId.ToString(), activity.SpanId.ToString(), activity.OperationName, activity.StartTimeUtc));
			};
			listener.ActivityStopped += activity =>
			{
				if (activity.GetBaggageItem(DiagnosticConstants.ShouldBeReceived) != true.ToString())
				{
					if (activity.GetTagItem(DiagnosticConstants.ShouldBeReceived) as bool? != true)
						return;
				}
				string traceId = activity.TraceId.ToString().TrimStart('0');
				SendActivityEnd(new ActivityEnd(options.Value.Service, traceId, activity.ParentSpanId.ToString(), activity.SpanId.ToString(), activity.OperationName, activity.StartTimeUtc, activity.StartTimeUtc + activity.Duration, activity.Status));
			};
			ActivitySource.AddActivityListener(listener);

		}

		public void SendLog<TState>(string handlerDisplayName, LogLevel logLevel, string traceId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var formattedMessage = formatter.Invoke(state, exception);
			foreach (var diagnosticProducer in logProducers)
			{
				try
				{
					diagnosticProducer.ProduceLog(new LogEntry(options.Value.Service, traceId, DateTimeOffset.UtcNow, logLevel, formattedMessage));
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"LogProducer: {diagnosticProducer.GetType().Name} failed to produce log with error {ex.Message}");
				}
			}
		}

		public void StartActivity(ActivityStart activityStart)
		{
			foreach (var producer in logProducers)
			{
				try
				{
					producer.StartActivity(activityStart);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"LogProducer: {producer.GetType().Name} failed to produce activity with error {ex.Message}");
				}
			}
		}

		private void SendActivityStart(ActivityStart activityStart)
		{
			foreach (var producer in logProducers)
			{
				try
				{
					producer.StartActivity(activityStart);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"LogProducer: {producer.GetType().Name} failed to produce activity start with error {ex.Message}");
				}
			}
		}

		private void SendActivityEnd(ActivityEnd activityEnd)
		{
			foreach (var producer in logProducers)
			{
				try
				{
					producer.EndActivity(activityEnd);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"LogProducer: {producer.GetType().Name} failed to produce activity end with error {ex.Message}");
				}
			}
		}
	}
}
