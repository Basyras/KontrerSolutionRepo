using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Broker.NetMQ
{
    public class WorkerRegistry : IWorkerRegistry
    {
        Dictionary<string, MessageWorkers> workerStorage = new Dictionary<string, MessageWorkers>();
        public void RegisterWorker(string workerId, string[] suppportedMessages)
        {
            foreach (var supportedMessage in suppportedMessages)
            {
                if (workerStorage.TryGetValue(supportedMessage, out var existingWorkerList))
                {
                    existingWorkerList.WorkerIds.Add(workerId);
                }
                else
                {
                    var newWorkerList = new List<string>();
                    newWorkerList.Add(workerId);
                    workerStorage.Add(supportedMessage, new MessageWorkers(supportedMessage, newWorkerList, 0));
                }
            }
        }

        public bool TryGetWorkerFor(string messageType, out string? workerId)
        {
            if (workerStorage.TryGetValue(messageType, out var workers))
            {
                if (workers.LastUsedWorkerId == workers.WorkerIds.Count - 1)
                    workers.LastUsedWorkerId = 0;
                else
                    workers.LastUsedWorkerId++;

                workerId = workers.WorkerIds[workers.LastUsedWorkerId];
                return true;
            }
            else
            {
                workerId = null;
                return false;
            }
        }

        public IEnumerable<string> GetWorkersFor(string messageType)
        {
            if (workerStorage.TryGetValue(messageType, out var workers))
            {
                return workers.WorkerIds;
            }
            else
            {
                throw new Exception("No worker what could consume this message");
            }
        }
    }
}
