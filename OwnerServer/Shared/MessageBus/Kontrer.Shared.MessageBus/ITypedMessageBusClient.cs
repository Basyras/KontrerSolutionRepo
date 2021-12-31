using Basyc.MessageBus.Shared;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
    public interface ITypedMessageBusClient : IDisposable
    {
        Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
              where TEvent : class, IEventMessage, new();

        Task PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken = default)
               where TEvent : notnull, IEventMessage;

        Task SendAsync<TCommand>(CancellationToken cancellationToken = default)
             where TCommand : class, IMessage, new();

        Task SendAsync<TCommand>(TCommand commandData, CancellationToken cancellationToken = default)
             where TCommand : notnull, IMessage;

        Task SendAsync(Type commandType, object commandData, CancellationToken cancellationToken = default);

        Task SendAsync(Type commandType, CancellationToken cancellationToken = default);

        Task<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
             where TRequest : class, IMessage<TResponse>, new()
             where TResponse : class;

        //ResponseType is not needed? it can be always accessed by responseObject.GetType();
        Task<object> RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken = default);

        Task<object> RequestAsync(Type requestType, object requestData, Type responseType, CancellationToken cancellationToken = default);

        Task<OneOf<TResponse, ErrorMessage>> RequestAsync<TRequest, TResponse>(TRequest requestData, CancellationToken cancellationToken = default)
            where TRequest : class, IMessage<TResponse>
            where TResponse : class;

        Task StartAsync(CancellationToken cancellationToken = default);
    }
}