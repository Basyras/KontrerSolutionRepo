using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus
{
    public interface IMessageBusManager
    {
        IReadOnlyCollection<BusSubscription> BusSubscriptions { get; }
        Task PublishAsync<TRequest>(TRequest data, CancellationToken cancellationToken = default, string topicName = null);
        void Subscribe<TRequest, TResponse>(Func<TRequest, Task> asyncHandler, string topicName = null);
        string BusName { get; }
        
        void LockSubscriptions();
    }
}
