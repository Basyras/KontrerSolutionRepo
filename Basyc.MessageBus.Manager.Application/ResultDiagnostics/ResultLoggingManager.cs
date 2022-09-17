using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class ResultLoggingManager : IResultLoggingManager
	{
		private readonly Dictionary<RequestResult, ResultLoggingContext> loggingContexts = new Dictionary<RequestResult, ResultLoggingContext>();

		public ResultLoggingManager(IEnumerable<ILogSource> logSources)
		{
			foreach (var logSource in logSources)
			{
				logSource.LogsReceived += LogSource_LogsReceived;
			}
		}

		private void LogSource_LogsReceived(object? sender, LogsUpdatedArgs e)
		{
			foreach (var logEntry in e.NewLogEntries)
			{
				var loggingContext = GetLoggingContext(logEntry.SessionId);
				loggingContext.AddLog(logEntry);
			}
		}

		public ResultLoggingContext RegisterLoggingContex(RequestResult requestResult)
		{
			ResultLoggingContext loggingContext = new ResultLoggingContext(requestResult);
			loggingContexts.Add(requestResult, loggingContext);
			return loggingContext;
		}

		public ResultLoggingContext GetLoggingContext(RequestResult requestResult)
		{
			return loggingContexts[requestResult];
		}

		public ResultLoggingContext GetLoggingContext(int requestResultId)
		{
			return loggingContexts.Values.First(x => x.RequestResult.Id == requestResultId);
		}

		public void FinishLoggingContext(RequestResult requestResult)
		{
			if (loggingContexts.Remove(requestResult) is false)
			{
				throw new ArgumentException("Coresponding context not found", nameof(requestResult));
			}
		}

	}
}
