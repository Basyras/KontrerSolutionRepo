namespace Basyc.Diagnostics.Receiving.Abstractions
{
	public interface IDiagnosticsLogReceiver
	{
		event EventHandler<LogsReceivedArgs> LogsReceived;
		Task StartReceiving();


	}
}
