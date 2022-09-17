using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Shared.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Client.Diagnostics.Sinks.BasycDiagnostics
{
	public class BasycDiagnosticsLogSink : ILogSink
	{
		private readonly ILogProducer[] logProducers;
		private readonly ILogger<BasycDiagnosticsLogSink> logger;

		public BasycDiagnosticsLogSink(IEnumerable<ILogProducer> logProducers, ILogger<BasycDiagnosticsLogSink> logger)
		{
			this.logProducers = logProducers.ToArray();
			this.logger = logger;
		}
		public void SendLog<TState>(string handlerDisplayName, LogLevel logLevel, int sessionId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var formattedMessage = formatter.Invoke(state, exception);
			foreach (var producer in logProducers)
			{
				try
				{
					producer.ProduceLog(new LogEntry(sessionId, DateTimeOffset.UtcNow, logLevel, formattedMessage));
				}
				catch (Exception ex)
				{
					logger.LogError(ex, $"LogProducer: {producer.GetType().Name} failed to produce log with error {ex.Message}");
				}
			}
		}
	}
}
