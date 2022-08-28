using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Client.Diagnostics
{
	public class BusHandlerLogger : ILogger
	{
		public IDisposable BeginScope<TState>(TState state)
		{
			throw new NotImplementedException();
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			throw new NotImplementedException();
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			throw new NotImplementedException();
		}
	}
}
