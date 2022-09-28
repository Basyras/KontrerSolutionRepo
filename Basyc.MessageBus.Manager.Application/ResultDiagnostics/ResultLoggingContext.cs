using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public class ResultLoggingContext
	{
		public List<LogEntry> LogEntries { get; } = new List<LogEntry>();

		public RequestResult RequestResult { get; }

		public ResultLoggingContext(RequestResult requestResult)
		{
			RequestResult = requestResult;
		}

		public void AddLog(LogLevel logLevel, string message)
		{
			AddLog(DateTimeOffset.UtcNow, logLevel, message);
		}

		public void AddLog(DateTimeOffset time, LogLevel logLevel, string message)
		{
			LogEntry newLogEntry = new(RequestResult.SessionId, time, logLevel, message);
			LogEntries.Add(newLogEntry);
			OnLogAdded(newLogEntry);
		}

		public void AddLog(LogEntry newLogEntry)
		{
			if (newLogEntry.SessionId != RequestResult.SessionId)
				throw new ArgumentException("Request id does not match context reuqest result id", nameof(newLogEntry));

			LogEntries.Add(newLogEntry);
			OnLogAdded(newLogEntry);
		}

		public event EventHandler<LogEntry>? LogAdded;
		private void OnLogAdded(LogEntry newLogEntry)
		{
			LogAdded?.Invoke(this, newLogEntry);
		}
	}
}
