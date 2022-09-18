﻿using Basyc.MessageBus.Client.Diagnostics;
using Basyc.MessageBus.Client.Diagnostics.Sinks;
using Microsoft.Extensions.Logging;

namespace Basyc.MessageBus.Client.Tests.Diagnostics
{
	public class BusHandlerLoggerTests
	{
		[Fact]
		public void Test()
		{
			int currectionSessionId = 1;
			string currentHandlerName = "testHandler";
			var normalLogger = new Mock<ILogger>();
			var logSink = new Mock<ILogSink>();
			logSink.Setup(x => x.SendLog<It.IsAnyType>(
				  It.IsAny<string>(),
				  It.IsAny<LogLevel>(),
				  It.IsAny<int>(),
				  It.IsAny<It.IsAnyType>(),
				  It.IsAny<Exception>(),
				  It.IsAny<Func<It.IsAnyType, Exception, string>>()))
			.Callback<string, LogLevel, int, object, Exception, Delegate>((handlerName, logLevel, sessionId, x, y, z) =>
			{
				sessionId.Should().Be(currectionSessionId);
				handlerName.Should().Be(currentHandlerName);
			});
			var handlerLogger = new BusHandlerLogger(normalLogger.Object, new ILogSink[] { logSink.Object }, currentHandlerName);
			//var handlerLoggerScope = handlerLogger.BeginHandlerScope(new HandlerScopeState(currectionSessionId));
			BusHandlerLoggerSessionManager.StartSession(1);
			handlerLogger.LogInformation("mess1");
		}
	}
}