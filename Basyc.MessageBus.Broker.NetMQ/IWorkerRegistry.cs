
namespace Basyc.MessageBus.Broker.NetMQ
{
    public interface IWorkerRegistry
    {
        IEnumerable<string> GetWorkersFor(string messageType);
        void RegisterWorker(string workerId, string[] suppportedMessages);
        bool TryGetWorkerFor(string messageType, out string? workerId);
    }
}