using Microsoft.Extensions.Logging;
using System;

namespace Basyc.MessageBus.Client.Diagnostics
{
	public record struct LogStorageKey(string HandlerDisplayName, EventId? EventId);
}
