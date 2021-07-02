using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Kontrer.Shared.MessageBus.PublishSubscribe;
using Kontrer.Shared.MessageBus.RequestResponse;
using Kontrer.Shared.MessageBus;

namespace Kontrer.Shared.MessageBus.MasstTransit
{
    public class MassTransitMessageBusManager : IMessageBusManager
    {
        private readonly IBusControl _massTransitBus;

        public MassTransitMessageBusManager(IBusControl massTransitBus)
        {
            _massTransitBus = massTransitBus;
        }

        public Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
        where TEvent : class, IBusEvent, new()
        {
            return _massTransitBus.Publish<TEvent>(cancellationToken);
        }

        public Task PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken = default)
                where TEvent : class, IBusEvent
        {
            return _massTransitBus.Publish(data, cancellationToken);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
             where TRequest : class, IRequest<TResponse>, new()
             where TResponse : class
        {
            var response = await _massTransitBus.Request<TRequest, TResponse>(cancellationToken);
            return response.Message;
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class
        {
            var response = await _massTransitBus.Request<TRequest, TResponse>(request, cancellationToken);
            return response.Message;
        }

        async Task IMessageBusManager.SendAsync<TRequest>(CancellationToken cancellationToken)
        {
            await _massTransitBus.Publish<TRequest>(cancellationToken);
        }

        async Task IMessageBusManager.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            await _massTransitBus.Publish(request, cancellationToken);
        }
    }
}