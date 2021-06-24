using Kontrer.OwnerServer.IdGeneratorService.Presentation.Abstraction;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Consumers
{
    public class AccommodationIdCreatedConsumer : IConsumer<AccommodationIdRequestedMessage>
    {
        private readonly IIdGeneratorManager _idGenerator;

        public AccommodationIdCreatedConsumer(IIdGeneratorManager idGenerator)
        {
            _idGenerator = idGenerator;
        }

        public async Task Consume(ConsumeContext<AccommodationIdRequestedMessage> context)
        {
            var newId = _idGenerator.CreateNewId(IIdGeneratorManager.OrdersGroupName);
            throw new NotImplementedException();
            //await context.RespondAsync<OrderStatusResult>(new
            //{
            //    OrderId = order.Id,
            //    order.Timestamp,
            //    order.StatusCode,
            //    order.StatusText
            //});
        }
    }
}