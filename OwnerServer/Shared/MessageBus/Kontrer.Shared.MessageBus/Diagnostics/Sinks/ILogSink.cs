using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Client.Diagnostics.Sinks
{
	public interface ILogSink
	{
		public void SendLog<TState>(string handlerDisplayName, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
	}
}