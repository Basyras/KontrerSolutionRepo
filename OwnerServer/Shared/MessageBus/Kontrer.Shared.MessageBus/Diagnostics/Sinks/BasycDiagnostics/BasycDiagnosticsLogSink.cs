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

		public BasycDiagnosticsLogSink(IEnumerable<ILogProducer> logProducers)
		{
			this.logProducers = logProducers.ToArray();
		}
		public void SendLog<TState>(string handlerDisplayName, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var formattedMessage = formatter.Invoke(state, exception);
			foreach (var producer in logProducers)
			{
				producer.ProduceLog(new LogEntry(0, DateTimeOffset.UtcNow, logLevel, formattedMessage));
			}
		}
	}
}
