
namespace Basyc.MessageBus.Broker.NetMQ
{
    public interface IWorkerRegistry
    {
        bool TryGetWorkersFor(string messageType, out string[] workerIds);
        void RegisterWorker(string workerId, string[] suppportedMessages);
        bool TryGetWorkerFor(string messageType, out string? workerId);
    }
}