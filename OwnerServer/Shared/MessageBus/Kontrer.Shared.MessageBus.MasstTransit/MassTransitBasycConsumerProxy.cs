using Basyc.MessageBus.RequestResponse;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.MasstTransit
{
    public class MassTransitBasycConsumerProxy<TRequest> : IConsumer<TRequest>
        where TRequest : class, IRequest
    {
        private readonly IRequestHandler<TRequest> requestHandler;

        public MassTransitBasycConsumerProxy(IRequestHandler<TRequest> requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            await requestHandler.Handle(context.Message, context.CancellationToken);
            await context.RespondAsync(new CommandResult());
            //context.Nori
        }
    }

    public class MassTransitBasycConsumerProxy<TRequest, TResponse> : IConsumer<TRequest>
     where TRequest : class, IRequest<TResponse>
     where TResponse : class
    {
        private readonly IRequestHandler<TRequest, TResponse> requestHandler;

        public MassTransitBasycConsumerProxy(IRequestHandler<TRequest, TResponse> requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            var response = await requestHandler.Handle(context.Message, context.CancellationToken);
            await context.RespondAsync(response);
        }
    }
}