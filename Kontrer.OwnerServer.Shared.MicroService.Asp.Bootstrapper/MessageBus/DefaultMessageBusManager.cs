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
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.MessageBus
{
    public class DefaultMessageBusManager : IMessageBusManager
    {
        private readonly DaprClient daprClient;
        private readonly IBusControl massTransitBus;
        private readonly IOptions<DaprMessageBusManagerOptions> options;
        private readonly JsonSerializerOptions serializerOptions;


#warning this manager may need options with pull handlers and register them while creating new instance of massTransitBus inside this ctor, or use options if massTransit has one
        public DefaultMessageBusManager(DaprClient daprClient, IBusControl massTransitBus, IOptions<DaprMessageBusManagerOptions> options, JsonSerializerOptions serializerOptions)
        {

            this.daprClient = daprClient;
            this.massTransitBus = massTransitBus;
            this.options = options;
            this.serializerOptions = serializerOptions;            
        }     

        public void RegisterConsumer<IConsumer>()
        {
            //massTransitBus.Saga
            massTransitBus.
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

        public Task PublishAsync<TRequest>(TRequest data, CancellationToken cancellationToken = default, string topicName = null)
            where TRequest : class
        {
            topicName ??= nameof(TRequest);
            //Can be implemented also with dapr
            return massTransitBus.Publish<TRequest>(data, cancellationToken);
        }
        public Task RequestAsync<TRequest>(CancellationToken cancellationToken = default)
              where TRequest : class, IRequest, new()
        {
            var request = new TRequest();
            return massTransitBus.Send<TRequest>(request, cancellationToken);
        }

        public Task RequestAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
            where TRequest : class, IRequest
        {
            return massTransitBus.Send<TRequest>(request, cancellationToken);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
             where TRequest : class, IRequest<TResponse>, new()
             where TResponse : class
        {
            var request = new TRequest();
            var response = await massTransitBus.Request<TRequest, TResponse>(request, cancellationToken);
            return response.Message;

        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class
        {
            var response = await massTransitBus.Request<TRequest, TResponse>(request, cancellationToken);
            return response.Message;
        }

     
    }
}
