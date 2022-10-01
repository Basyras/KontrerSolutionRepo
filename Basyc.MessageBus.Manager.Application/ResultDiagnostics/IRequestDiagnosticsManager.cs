using Basyc.Diagnostics.Shared.Durations;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface IRequestDiagnosticsManager
	{
		RequestDiagnosticsContext RegisterRequest(RequestResult requestResult, DurationMapBuilder durationMapBuilder);
		RequestDiagnosticsContext GetContext(RequestResult requestResult);
		RequestDiagnosticsContext GetContextByTraceId(string traceId);
	}
}