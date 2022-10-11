using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class RequestDiagnosticsManager : IRequestDiagnosticsManager
	{
		private readonly Dictionary<string, RequestDiagnosticContext> traceIdToContextMap = new();

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
				var loggingContext = GetDiagnostics(activityStart.TraceId);
				loggingContext.StartActivity(activityStart);
			}
		}

		private void LogSource_ActivityEndsReceived(object? sender, ActivityEndsReceivedArgs e)
		{
			foreach (var activityEnd in e.ActivityEnds)
			{
				var loggingContext = GetDiagnostics(activityEnd.TraceId);
				loggingContext.EndActivity(activityEnd);
			}
		}

		private void LogSource_LogsReceived(object? sender, LogsUpdatedArgs e)
		{
			foreach (var logEntry in e.NewLogEntries)
			{
				var loggingContext = GetDiagnostics(logEntry.TraceId);
				loggingContext.Log(logEntry);
			}
		}

		public RequestDiagnosticContext CreateDiagnostics(string traceId)
		{
			RequestDiagnosticContext loggingContext = new RequestDiagnosticContext(traceId);
			traceIdToContextMap.Add(traceId, loggingContext);
			return loggingContext;
		}

		public RequestDiagnosticContext GetDiagnostics(string traceId)
		{
			return traceIdToContextMap[traceId];
		}
	}
}
