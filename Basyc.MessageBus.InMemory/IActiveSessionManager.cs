namespace Basyc.MessageBus.Client.NetMQ
{
    public interface IActiveSessionManager
    {
        ActiveSession CreateSession();
        bool TryCompleteSession(int sessionId, object sessionResult);
    }
}