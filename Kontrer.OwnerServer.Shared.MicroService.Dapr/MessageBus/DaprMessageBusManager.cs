using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using FluentAssertions;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Kontrer.OwnerServer.Shared.MicroService.Dapr.MessageBus
{
    public class DaprMessageBusManager : MessageBusManagerBase
    {
        private readonly DaprClient daprClient;
        private readonly IOptions<DaprMessageBusManagerOptions> options;
        private readonly JsonSerializerOptions serializerOptions;

        public DaprMessageBusManager(DaprClient daprClient, IOptions<DaprMessageBusManagerOptions> options, JsonSerializerOptions serializerOptions)
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

        public override void Subscribe<TRequest, TResponse>(Func<TRequest, Task> asyncHandler, string topicName = null)
        {
            IsSubscriptionLocked.Should().BeFalse("Can not add new subcription after StartSubscribtions has been called");
            Microsoft.AspNetCore.Http.RequestDelegate handler = async (HttpContext context) =>
            {
                TRequest requestData = await JsonSerializer.DeserializeAsync<TRequest>(context.Request.Body, serializerOptions);
                await asyncHandler.Invoke(requestData);
            };
            tempSubscriptions.Add(new BusSubscription(topicName, typeof(TRequest), handler));
        }
    }
}
