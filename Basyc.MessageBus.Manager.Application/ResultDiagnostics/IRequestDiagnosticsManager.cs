namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface IRequestDiagnosticsManager
	{
		RequestDiagnosticContext CreateDiagnostics(string tracId);
		RequestDiagnosticContext GetDiagnostics(string traceId);
	}
}