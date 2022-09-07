using Microsoft.Extensions.Logging;

namespace Basyc.MessageBus.Client.Diagnostics.Sinks.Http
{
	public record LogMessageDTO(LogLevel LogLevel, EventId EventId, string Message);
}
