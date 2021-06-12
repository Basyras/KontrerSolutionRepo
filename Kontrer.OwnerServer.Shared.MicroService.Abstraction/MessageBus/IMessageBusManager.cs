using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.PublishSubscribe;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse;
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
        Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
              where TEvent : IBusEvent, new();

        Task PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken = default)
               where TEvent : IBusEvent;



        Task<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
             where TRequest : class, IRequest<TResponse>, new()
             where TResponse : class;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class;







    }
}
