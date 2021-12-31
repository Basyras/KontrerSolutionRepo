namespace Basyc.MessageBus.Client.NetMQ
{
    public interface IActiveSessionManager
    {
        ActiveSession CreateSession(string messageType);
        bool TryCompleteSession(int sessionId, object sessionResult);
    }
}