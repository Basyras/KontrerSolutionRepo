using Basyc.Diagnostics.Shared.Durations;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface IRequestDiagnosticsManager
	{
		RequestDiagnosticsContext RegisterRequest(RequestResultContext requestResult, DurationMapBuilder durationMapBuilder);
		RequestDiagnosticsContext GetContext(RequestResultContext requestResult);
		RequestDiagnosticsContext GetContextByTraceId(string traceId);
	}
}