using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Client.Diagnostics
{
	public record struct LogStorageKey(Type HandlerType, EventId? EventId);
}
