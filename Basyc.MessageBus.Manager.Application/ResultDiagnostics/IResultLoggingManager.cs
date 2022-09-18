namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface IResultLoggingManager
	{
		ResultLoggingContext RegisterLoggingContex(RequestResult requestResult);
		void AddSessionToContext(RequestResult requestResult, int sessionId);

		ResultLoggingContext GetLoggingContext(RequestResult requestResult);
		ResultLoggingContext GetLoggingContext(int sessionId);

		void FinishLoggingContext(RequestResult requestResult);
	}
}