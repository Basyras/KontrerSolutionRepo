using Basyc.Diagnostics.Shared.Logging;

namespace Basyc.Diagnostics.Server.Abstractions
{
	public interface IDiagnosticsServer
	{
		Task ReceiveLogs(LogEntry[] logEntries);
		Task ReceiveStartedActivities(ActivityStart[] startedActivity);
		Task ReceiveEndedActivities(ActivityEnd[] activities);
	}
}