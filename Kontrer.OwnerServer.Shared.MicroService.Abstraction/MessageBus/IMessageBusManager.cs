using MediatR;
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
        void RegisterSubscribe<TRequest, TResponse>(Func<TRequest, Task> asyncHandler, string topicName = null);

        Task PushAsync<TRequest>(TRequest request, CancellationToken cancellationToken);
        void RegisterPull<TRequest>(Action<TRequest> pullHandler);

        string BusName { get; }
        Task RequestAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;        
        Task<TResponse> RequestAsync<TRequest,TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest<TResponse>;        
    }
}
