using Dapr.Client;
using FluentAssertions;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Dapr.MessageBus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.MessageBus
{
    public class DefaultMessageBusManager : MessageBusManagerBase
    {
        private readonly DaprClient daprClient;
        private readonly IOptions<DaprMessageBusManagerOptions> options;
        private readonly JsonSerializerOptions serializerOptions;

        public DefaultMessageBusManager(DaprClient daprClient, IOptions<DaprMessageBusManagerOptions> options, JsonSerializerOptions serializerOptions)
        {
            
            this.daprClient = daprClient;
            this.options = options;
            this.serializerOptions = serializerOptions;
            BusName = options.Value.PubSubName;
        }

        public override string BusName { get; }

        public override Task PublishAsync<TRequest>(TRequest data, CancellationToken cancellationToken = default, string topicName = null)
        {
            topicName ??= nameof(TRequest);
            return daprClient.PublishEventAsync(BusName, topicName, data, cancellationToken);            
        }
        public override void RegisterSubscribe<TRequest, TResponse>(Func<TRequest, Task> asyncHandler, string topicName = null)
        {
            IsSubscriptionLocked.Should().BeFalse("Can not add new subcription after StartSubscribtions has been called");
            Microsoft.AspNetCore.Http.RequestDelegate handler = async (HttpContext context) =>
            {
                TRequest requestData = await JsonSerializer.DeserializeAsync<TRequest>(context.Request.Body, serializerOptions);
                await asyncHandler.Invoke(requestData);
            };
            tempSubscriptions.Add(new BusSubscription(topicName, typeof(TRequest), handler));
        }




        public override Task PushAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void RegisterPull<TRequest>(Action<TRequest> pullHandler)
        {
            throw new NotImplementedException();
        }      



        public override Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request,CancellationToken cancellationToken)
        {
            //daprClient.InvokeMethodAsync<TResponse>()
            throw new NotImplementedException();
        }

        public override Task RequestAsync<TRequest>(TRequest request,CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
