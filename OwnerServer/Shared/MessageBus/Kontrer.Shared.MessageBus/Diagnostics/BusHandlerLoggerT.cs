using Basyc.MessageBus.Client.Diagnostics.Sinks;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.Shared.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Client.Diagnostics
{
	public class BusHandlerLoggerT<TCategory> : ILogger<TCategory>
	{
		private readonly ILogger<TCategory> logger;
		private readonly ILogSink[] logSinks;
		private readonly bool shouldLogInLogStorage;

		public BusHandlerLoggerT(IEnumerable<ILogSink> logSinks, ILoggerFactory factory)
		{
			logger = new Logger<TCategory>(factory);
			this.logSinks = logSinks.ToArray();
			shouldLogInLogStorage = GenericsHelper.IsAssignableToGenericType<TCategory>(typeof(IMessageHandler<>))
				|| GenericsHelper.IsAssignableToGenericType<TCategory>(typeof(IMessageHandler<,>));
		}
		public IDisposable BeginScope<TState>(TState state)
		{
			return logger.BeginScope(state);
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return logger.IsEnabled(logLevel);
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			logger.Log(logLevel, eventId, state, exception, formatter);
			if (shouldLogInLogStorage)
			{
				foreach (var logSink in logSinks)
				{
					logSink.SendLog(nameof(TCategory), logLevel, eventId, state, exception, formatter);
				}
			}
		}
	}
}