using Kontrer.OwnerServer.OrderService.Application.Accommodation;
using Kontrer.OwnerServer.OrderService.Client.Requests;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Accommodation.Consumers
{
    public class CreateNewAccommodationOrderConsumer : IConsumer<CreateNewOrderRequest>
    {
        private readonly AccommodationOrderManager _accommodationOrderManager;

        public CreateNewAccommodationOrderConsumer(AccommodationOrderManager accommodationOrderManager)
        {
            _accommodationOrderManager = accommodationOrderManager;
        }

        public Task Consume(ConsumeContext<CreateNewOrderRequest> context)
        {
            return _accommodationOrderManager.CreateOrderAsync(context.Message.CustomerId, context.Message.Blueprint, context.Message.OrderCulture);
        }
    }
}