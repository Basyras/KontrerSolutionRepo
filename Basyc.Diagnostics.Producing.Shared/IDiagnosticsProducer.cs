using Basyc.Diagnostics.Shared.Logging;

namespace Basyc.Diagnostics.Producing.Shared
{
	public interface IDiagnosticsProducer
	{
		Task ProduceLog(LogEntry logEntry);
		Task StartActivity(ActivityStart activity);
		Task EndActivity(ActivityEnd activity);
	}
}