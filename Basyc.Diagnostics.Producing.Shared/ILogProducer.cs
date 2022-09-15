using Basyc.Diagnostics.Shared.Logging;

namespace Basyc.Diagnostics.Producing.Shared
{
	public interface ILogProducer
	{
		Task ProduceLog(LogEntry logEntry);
	}
}