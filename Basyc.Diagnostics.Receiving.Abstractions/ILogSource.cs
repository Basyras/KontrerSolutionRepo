namespace Basyc.Diagnostics.Receiving.Abstractions
{
	public interface ILogSource
	{
		event EventHandler<LogsReceivedArgs> LogsReceived;
	}
}
