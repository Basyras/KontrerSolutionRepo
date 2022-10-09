namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface IRequestDiagnosticsManager
	{
		RequestDiagnostics CreateDiagnostics(string tracId);
		RequestDiagnostics GetDiagnostics(string traceId);
	}
}