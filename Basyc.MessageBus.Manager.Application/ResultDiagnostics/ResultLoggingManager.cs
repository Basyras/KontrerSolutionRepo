using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class ResultLoggingManager : IResultLoggingManager
	{
		private readonly Dictionary<RequestResult, ResultLoggingContext> resultToContextMap = new Dictionary<RequestResult, ResultLoggingContext>();
		private readonly Dictionary<int, ResultLoggingContext> sesionIdToContextMap = new();

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
			resultToContextMap.Add(requestResult, loggingContext);
			return loggingContext;
		}

		public ResultLoggingContext GetLoggingContext(RequestResult requestResult)
		{
			return resultToContextMap[requestResult];
		}

		public ResultLoggingContext GetLoggingContext(int requestResultId)
		{
			return resultToContextMap.Values.First(x => x.RequestResult.Id == requestResultId);
		}

		public void FinishLoggingContext(RequestResult requestResult)
		{
			if (resultToContextMap.Remove(requestResult) is false)
			{
				throw new ArgumentException("Coresponding context not found", nameof(requestResult));
			}
		}

		public void AddSessionToContext(RequestResult requestResult, int sessionId)
		{
			sesionIdToContextMap.Add(sessionId, resultToContextMap[requestResult]);
		}
	}
}
