namespace Basyc.MessageBus.Manager.Infrastructure.Building
{
	public class NullBasycDiagnosticsReceiverSessionMapper : IBasycDiagnosticsReceiverSessionMapper
	{
		public int GetSessionId(int sessionId)
		{
			return sessionId;
		}
	}
}