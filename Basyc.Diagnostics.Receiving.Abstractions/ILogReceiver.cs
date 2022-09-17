namespace Basyc.Diagnostics.Receiving.Abstractions
{
	public interface ILogReceiver
	{
		event EventHandler<LogsReceivedArgs> LogsReceived;
		Task StartReceiving();


	}
}
