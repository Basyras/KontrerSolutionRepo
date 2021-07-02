using Kontrer.Shared.MessageBus.RequestResponse;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.MasstTransit
{
    public class MassTransitGenericConsumerProxy<TRequest> : IConsumer<TRequest>
        where TRequest : class, IRequest
    {
        private readonly IRequestHandler<TRequest> requestHandler;

        public MassTransitGenericConsumerProxy(IRequestHandler<TRequest> requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        public Task Consume(ConsumeContext<TRequest> context)
        {
            return requestHandler.Handle(context.Message);
        }
    }

    public class MassTransitGenericConsumerProxy<TRequest, TResponse> : IConsumer<TRequest>
     where TRequest : class, IRequest<TResponse>
     where TResponse : class
    {
        private readonly IRequestHandler<TRequest, TResponse> requestHandler;

        public MassTransitGenericConsumerProxy(IRequestHandler<TRequest, TResponse> requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            var response = await requestHandler.Handle(context.Message);
            await context.RespondAsync(response);
        }
    }
}