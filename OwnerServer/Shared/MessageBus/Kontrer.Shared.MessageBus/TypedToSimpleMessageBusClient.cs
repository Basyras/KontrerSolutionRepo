using Basyc.MessageBus.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
    public class TypedToSimpleMessageBusClient : ITypedMessageBusClient
    {
        private readonly ISimpleMessageBusClient messageBusClient;

        public TypedToSimpleMessageBusClient(ISimpleMessageBusClient messageBusClient)
        {
            this.messageBusClient = messageBusClient;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return messageBusClient.StartAsync(cancellationToken);
        }

        void IDisposable.Dispose()
        {
            messageBusClient.Dispose();
        }

        Task ITypedMessageBusClient.PublishAsync<TEvent>(CancellationToken cancellationToken)
        {
            return messageBusClient.PublishAsync(TypedToSimpleConverter.ConvertTypeToSimple<TEvent>(), cancellationToken);
        }

        Task ITypedMessageBusClient.PublishAsync<TEvent>(TEvent eventData, CancellationToken cancellationToken)
        {
            return messageBusClient.PublishAsync(TypedToSimpleConverter.ConvertTypeToSimple<TEvent>(),eventData, cancellationToken);
        }

        async Task<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken)
        {
            var genericTask = messageBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple<TRequest>(),cancellationToken);
            //return (TResponse)((dynamic)genericTask).Result;
            //return genericTask.ContinueWith(x=> (TResponse)x.Result);
            return (TResponse) await genericTask.ConfigureAwait(false);

        }

        Task<object> ITypedMessageBusClient.RequestAsync(Type requestType, Type responseType, CancellationToken cancellationToken)
        {
            var genericTask = messageBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple(requestType), cancellationToken);
            return genericTask;
        }

        Task<object> ITypedMessageBusClient.RequestAsync(Type requestType, object requestData, Type responseType, CancellationToken cancellationToken)
        {
            var genericTask = messageBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple(requestType),requestData, cancellationToken);
            return genericTask;
        }

        async Task<TResponse> ITypedMessageBusClient.RequestAsync<TRequest, TResponse>(TRequest requestData, CancellationToken cancellationToken)
        {
            var genericTask = messageBusClient.RequestAsync(TypedToSimpleConverter.ConvertTypeToSimple<TRequest>(), requestData, cancellationToken);
            return (TResponse) await genericTask.ConfigureAwait(false);
        }


        Task ITypedMessageBusClient.SendAsync<TCommand>(CancellationToken cancellationToken)
        {
            return messageBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple<TCommand>(), cancellationToken);
        }

        Task ITypedMessageBusClient.SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
        {
            return messageBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple<TCommand>(), command, cancellationToken);
        }

        Task ITypedMessageBusClient.SendAsync(Type commandType, object command, CancellationToken cancellationToken)
        {
            return messageBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple(commandType), command, cancellationToken);
        }

        Task ITypedMessageBusClient.SendAsync(Type commandType, CancellationToken cancellationToken)
        {
            return messageBusClient.SendAsync(TypedToSimpleConverter.ConvertTypeToSimple(commandType), cancellationToken);
        }

    }
}
