using Basyc.MessageBus.Client.RequestResponse;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Client.Diagnostics
{
	public class BusHandlerLoggerT<TCategory> : ILogger<TCategory>
	{
		private readonly ILogger<TCategory> logger;
		private readonly ITemporaryLogStorage logStorage;
		private readonly bool shouldLogInLogStorage;

		public BusHandlerLoggerT(ITemporaryLogStorage logStorage, ILoggerFactory factory)
		{
			logger = new Logger<TCategory>(factory);
			this.logStorage = logStorage;
			shouldLogInLogStorage = typeof(TCategory).IsAssignableFrom(typeof(IMessageHandler<>)) || typeof(TCategory).IsAssignableFrom(typeof(IMessageHandler<,>));
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
			if (shouldLogInLogStorage)
			{
				var message = formatter.Invoke(state, exception);
				logStorage.AddLog(new LogStorageKey(typeof(TCategory), eventId), message);
			}
			logger.Log(logLevel, eventId, state, exception, formatter);
		}
	}
}