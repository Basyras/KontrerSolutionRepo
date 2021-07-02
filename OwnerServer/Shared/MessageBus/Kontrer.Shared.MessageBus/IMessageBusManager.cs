using Kontrer.Shared.MessageBus.PublishSubscribe;
using Kontrer.Shared.MessageBus.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus
{
    public interface IMessageBusManager
    {
        Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
              where TEvent : class, IBusEvent, new();

        Task PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken = default)
               where TEvent : class, IBusEvent;

        Task SendAsync<TRequest>(CancellationToken cancellationToken = default)
             where TRequest : class, IRequest, new();

        Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
             where TRequest : class, IRequest;

        Task<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
             where TRequest : class, IRequest<TResponse>, new()
             where TResponse : class;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class;
    }
}