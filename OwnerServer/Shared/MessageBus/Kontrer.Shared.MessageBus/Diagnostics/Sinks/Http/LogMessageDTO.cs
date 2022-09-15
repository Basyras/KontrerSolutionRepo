using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Client.Diagnostics.Sinks.Http
{
	public record LogMessageDTO(LogLevel LogLevel, DateTimeOffset time, EventId EventId, string Message);
}
