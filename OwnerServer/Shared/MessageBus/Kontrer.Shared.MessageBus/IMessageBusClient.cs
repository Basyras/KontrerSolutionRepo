using Basyc.MessageBus.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
    public interface IMessageBusClient: IDisposable
    {
        Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
              where TEvent : class, IEventMessage, new();

        Task PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken = default)
               where TEvent : notnull, IEventMessage;

        Task SendAsync<TCommand>(CancellationToken cancellationToken = default)
             where TCommand : class, IMessage, new();

        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
             where TCommand : notnull, IMessage;

        Task SendAsync(Type commandType, object command, CancellationToken cancellationToken = default);

        Task SendAsync(Type commandType, CancellationToken cancellationToken = default);

        Task<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
             where TRequest : class, IMessage<TResponse>, new()
             where TResponse : class;

        Task<object> RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken = default);

        Task<object> RequestAsync(Type requestType, object request, Type responseType, CancellationToken cancellationToken = default);

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IMessage<TResponse>
            where TResponse : class;
    }
}