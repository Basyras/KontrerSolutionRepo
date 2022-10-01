﻿using Basyc.MessageBus.Client.Diagnostics.Sinks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Client.Diagnostics
{
	public class DummyLoggerCategory { };

	public class BusHandlerLogger : ILogger
	{
		private readonly ILogger normalLogger;
		private readonly IBusClientLogExporter[] logSinks;

		public BusHandlerLogger(ILogger normalLogger, IEnumerable<IBusClientLogExporter> logSinks)
		{
			this.normalLogger = normalLogger;
			this.logSinks = logSinks.ToArray();
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			var normalLoggerNewScope = normalLogger.BeginScope(state);
			return normalLoggerNewScope;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (BusHandlerLoggerSessionManager.HasSession(out var session) is false)
			{
				throw new InvalidOperationException($"Can't log without starting {nameof(BusHandlerLoggerSessionManager.StartSession)}. This logger should be only used for bus handlers");
			}

			if (normalLogger.IsEnabled(logLevel))
			{
				normalLogger.Log(logLevel, eventId, state, exception, formatter);
			}

			foreach (var logSink in logSinks)
			{
				logSink.SendLog(session.HandlerName, logLevel, session.TraceId, state, exception, formatter);
			}
		}
	}

	public class BusHandlerLogger<THandler> : BusHandlerLogger, ILogger<THandler>
	{
		public BusHandlerLogger(ILogger<THandler> normalLogger, IEnumerable<IBusClientLogExporter> logSinks) : base(normalLogger, logSinks)
		{
		}
	}
}
