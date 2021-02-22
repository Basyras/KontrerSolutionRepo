using Kontrer.OwnerServer.IdGeneratorService.Presentation.Abstraction;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Consumers
{
    public class AccommodationIdCreatedConsumer : IConsumer<AccommodationIdRequestedMessage>
    {
        public Task Consume(ConsumeContext<AccommodationIdRequestedMessage> context)
        {                      
            return Task.CompletedTask;
        }
    }
}
