using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public record LogEntry(int RequestId, DateTimeOffset Time, LogLevel LogLevel, string Message);

}
