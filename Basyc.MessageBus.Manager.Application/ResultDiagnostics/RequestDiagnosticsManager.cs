using Basyc.Diagnostics.Shared.Durations;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class RequestDiagnosticsManager : IRequestDiagnosticsManager
	{
		private readonly Dictionary<RequestResult, RequestDiagnosticsContext> resultToContextMap = new Dictionary<RequestResult, RequestDiagnosticsContext>();
		private readonly Dictionary<string, RequestDiagnosticsContext> traceIdToContextMap = new();

		public RequestDiagnosticsManager(IEnumerable<IRequestDiagnosticsSource> logSources)
		{
			foreach (var logSource in logSources)
			{
				logSource.LogsReceived += LogSource_LogsReceived;
				logSource.ActivitiesReceived += LogSource_ActivitiesReceived;
			}
		}

		private void LogSource_ActivitiesReceived(object? sender, ActivitesUpdatedArgs e)
		{
			foreach (var activity in e.NewActivities)
			{
				var loggingContext = GetContextByTraceId(activity.TraceId);
				loggingContext.AddActivity(activity);
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

		public RequestDiagnosticsContext RegisterRequest(RequestResult requestResult, DurationMapBuilder durationMapBuilder)
		{
			RequestDiagnosticsContext loggingContext = new RequestDiagnosticsContext(requestResult, durationMapBuilder);
			resultToContextMap.Add(requestResult, loggingContext);
			traceIdToContextMap.Add(requestResult.TraceId, loggingContext);
			return loggingContext;
		}

		public RequestDiagnosticsContext GetContext(RequestResult requestResult)
		{
			return resultToContextMap[requestResult];
		}

		public RequestDiagnosticsContext GetContextByTraceId(string traceId)
		{
			return traceIdToContextMap[traceId];
		}
	}
}
