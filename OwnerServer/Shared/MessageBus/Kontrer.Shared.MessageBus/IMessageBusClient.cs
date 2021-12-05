using Basyc.MessageBus.PublishSubscribe;
using Basyc.MessageBus.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus
{
    public interface IMessageBusClient
    {
        Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
              where TEvent : class, IBusEvent, new();

        Task PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken = default)
               where TEvent : notnull, IBusEvent;

        Task SendAsync<TCommand>(CancellationToken cancellationToken = default)
             where TCommand : class, IRequest, new();

        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
             where TCommand : notnull, IRequest;

        Task SendAsync(Type commandType, object command, CancellationToken cancellationToken = default);

        Task SendAsync(Type commandType, CancellationToken cancellationToken = default);

        Task<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
             where TRequest : class, IRequest<TResponse>, new()
             where TResponse : class;

        Task<object> RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken = default);

        Task<object> RequestAsync(Type requestType, object request, Type responseType, CancellationToken cancellationToken = default);

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class;
    }
}