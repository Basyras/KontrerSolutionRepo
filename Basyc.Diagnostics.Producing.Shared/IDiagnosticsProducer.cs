using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Logging;
using System.Diagnostics;

namespace Basyc.Diagnostics.Producing.Shared
{
	public interface IDiagnosticsProducer
	{
		Task ProduceLog(LogEntry logEntry);
		Task ProduceActivityEnd(ActivityEntry activity);
	}
}