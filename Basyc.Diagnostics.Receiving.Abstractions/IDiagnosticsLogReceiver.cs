namespace Basyc.Diagnostics.Receiving.Abstractions
{
	public interface IDiagnosticsLogReceiver
	{
		event EventHandler<LogsReceivedArgs> LogsReceived;
		event EventHandler<ActivitiesReceivedArgs> ActivitiesReceived;
		Task StartReceiving();


	}
}
