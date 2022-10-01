namespace Basyc.MessageBus.Client.NetMQ.Sessions
{
	public interface ISessionManager<TSessionResult>
	{
		NetMqSession<TSessionResult> CreateSession(string messageType);
		bool TryCompleteSession(int sessionId, TSessionResult sessionResult);
	}
}