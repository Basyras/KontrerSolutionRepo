namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface IRequestDiagnosticsManager
	{
		RequestDiagnosticContext CreateDiagnostics(string traceId);
		RequestDiagnosticContext GetDiagnostics(string traceId);
	}
}