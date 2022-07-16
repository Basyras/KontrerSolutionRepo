namespace Basyc.MessageBus.Client.NetMQ.Sessions
{
    public interface ISessionManager<TSessionResult>
    {
        Session<TSessionResult> CreateSession(string messageType);
        bool TryCompleteSession(int sessionId, TSessionResult sessionResult);
    }
}