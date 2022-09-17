using Basyc.Diagnostics.Shared.Logging;

namespace Basyc.Diagnostics.Server.Abstractions
{
	public interface IDiagnosticsServer
	{
		Task ReceiveLogs(LogEntry[] logEntries);
		Task ReceiveLogEntriesFromProducers(LogEntry[] logEntries);
	}
}