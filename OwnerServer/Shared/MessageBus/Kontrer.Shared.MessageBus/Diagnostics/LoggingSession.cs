namespace Basyc.MessageBus.Client.Diagnostics
{
	public readonly record struct LoggingSession(int SessionId, string HandlerName);
}
