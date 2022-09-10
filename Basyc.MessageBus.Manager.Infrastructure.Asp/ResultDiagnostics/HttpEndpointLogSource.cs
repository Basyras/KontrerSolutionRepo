using Basyc.MessageBus.Manager.Application.ResultDiagnostics;

namespace Basyc.MessageBus.Manager.Infrastructure.Asp.ResultDiagnostics
{
	public class HttpEndpointLogSource : ILogSource
	{
		public event EventHandler<LogsReceivedArgs>? LogsReceived;
	}
}
