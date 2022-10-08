namespace Basyc.Diagnostics.Receiving.Abstractions
{
	public interface IDiagnosticsLogReceiver
	{
		event EventHandler<LogsReceivedArgs> LogsReceived;
		event EventHandler<ActivityEndsReceivedArgs> ActivityEndsReceived;
		event EventHandler<ActivityStartsReceivedArgs> ActivityStartsReceived;
		Task StartReceiving();


	}
}
