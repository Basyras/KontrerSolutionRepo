using Basyc.Diagnostics.Shared.Durations;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class RequestDiagnosticsManager : IRequestDiagnosticsManager
	{
		private readonly Dictionary<RequestResultContext, RequestDiagnosticsContext> resultToContextMap = new Dictionary<RequestResultContext, RequestDiagnosticsContext>();
		private readonly Dictionary<string, RequestDiagnosticsContext> traceIdToContextMap = new();

		public RequestDiagnosticsManager(IEnumerable<IRequestDiagnosticsSource> logSources)
		{
			foreach (var logSource in logSources)
			{
				logSource.LogsReceived += LogSource_LogsReceived;
				logSource.ActivityStartsReceived += LogSource_ActivityStartsReceived;
				logSource.ActivityEndsReceived += LogSource_ActivityEndsReceived;
			}
		}



		private void LogSource_ActivityStartsReceived(object? sender, ActivityStartsReceivedArgs e)
		{
			foreach (var activityStart in e.ActivityStarts)
			{
				var loggingContext = GetContextByTraceId(activityStart.TraceId);
				loggingContext.StartActivity(activityStart);
			}
		}

		private void LogSource_ActivityEndsReceived(object? sender, ActivityEndsReceivedArgs e)
		{
			foreach (var activityEnd in e.ActivityEnds)
			{
				var loggingContext = GetContextByTraceId(activityEnd.TraceId);
				loggingContext.EndActivity(activityEnd);
			}
		}

		private void LogSource_LogsReceived(object? sender, LogsUpdatedArgs e)
		{
			foreach (var logEntry in e.NewLogEntries)
			{
				var loggingContext = GetContextByTraceId(logEntry.TraceId);
				loggingContext.AddLog(logEntry);
			}
		}

		public RequestDiagnosticsContext RegisterRequest(RequestResultContext requestResult, DurationMapBuilder durationMapBuilder)
		{
			RequestDiagnosticsContext loggingContext = new RequestDiagnosticsContext(requestResult, durationMapBuilder);
			resultToContextMap.Add(requestResult, loggingContext);
			traceIdToContextMap.Add(requestResult.TraceId, loggingContext);
			return loggingContext;
		}

		public RequestDiagnosticsContext GetContext(RequestResultContext requestResult)
		{
			return resultToContextMap[requestResult];
		}

		public RequestDiagnosticsContext GetContextByTraceId(string traceId)
		{
			return traceIdToContextMap[traceId];
		}
	}
}
