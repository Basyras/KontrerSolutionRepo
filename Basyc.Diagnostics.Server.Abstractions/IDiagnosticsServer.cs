using Basyc.Diagnostics.Shared.Logging;
using System.Diagnostics;

namespace Basyc.Diagnostics.Server.Abstractions
{
	public interface IDiagnosticsServer
	{
		Task ReceiveLogs(LogEntry[] logEntries);
		Task ReceiveActivities(ActivityEntry[] activities);
	}
}