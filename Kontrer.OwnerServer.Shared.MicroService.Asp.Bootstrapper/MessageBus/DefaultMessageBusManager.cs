using Dapr.Client;
using FluentAssertions;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Dapr.MessageBus;
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
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.PublishSubscribe;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.MessageBus
{
    public class DefaultMessageBusManager : IMessageBusManager
    {
        private readonly IBusControl _massTransitBus;
        private readonly IOptions<DaprMessageBusManagerOptions> options;
        private readonly JsonSerializerOptions serializerOptions;

#warning this manager may need options with pull handlers and register them while creating new instance of massTransitBus inside this ctor, or use options if massTransit has one

        public DefaultMessageBusManager(IBusControl massTransitBus, IOptions<DaprMessageBusManagerOptions> options, JsonSerializerOptions serializerOptions)
        {
            _massTransitBus = massTransitBus;
            this.options = options;
            this.serializerOptions = serializerOptions;
        }

        //public void RegisterSubscribe<TResponse>(Func<TResponse, Task> asyncHandler, string topicName = null)
        //{
        //    //Dapr implementation
        //    //IsSubscriptionLocked.Should().BeFalse("Can not add new subcription after StartSubscribtions has been called");
        //    //Microsoft.AspNetCore.Http.RequestDelegate handler = async (HttpContext context) =>
        //    //{
        //    //    TResponse requestData = await JsonSerializer.DeserializeAsync<TResponse>(context.Request.Body, serializerOptions);
        //    //    await asyncHandler.Invoke(requestData);
        //    //};
        //    //tempSubscriptions.Add(new BusSubscription(topicName, typeof(TResponse), handler));

        //    //massTransitBus.ConnectConsumer<>()
        //}

        public Task PublishAsync<TEvent>(CancellationToken cancellationToken = default)
        where TEvent : class, IBusEvent, new()
        {
            return _massTransitBus.Publish<TEvent>(cancellationToken);
        }

        public Task PublishAsync<TEvent>(TEvent data, CancellationToken cancellationToken = default)
                where TEvent : class, IBusEvent
        {
            return _massTransitBus.Publish<TEvent>(data, cancellationToken);
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
    }
}