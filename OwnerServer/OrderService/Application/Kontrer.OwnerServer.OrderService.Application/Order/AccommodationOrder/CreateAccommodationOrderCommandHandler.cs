using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrders;
using Kontrer.OwnerServer.OrderService.Dtos.Models;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.Shared.DomainDrivenDesign.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder
{
    public class CreateAccommodationOrderCommandHandler : CommandHandlerBase<CreateAccommodationOrderCommand, CreateAccommodationOrderResponse>
    {
        private readonly IAccommodationOrderRepository orderRepository;
        private readonly IMessageBusManager messageBus;

        public CreateAccommodationOrderCommandHandler(IAccommodationOrderRepository orderRepository, IMessageBusManager messageBus)
        {
            this.orderRepository = orderRepository;
            this.messageBus = messageBus;
        }

        public override async Task<CreateAccommodationOrderResponse> Handle(CreateAccommodationOrderCommand command, CancellationToken cancellationToken = default)
        {
            var response = await messageBus.RequestAsync<CreateAccommodationOrderCommand, CreateAccommodationOrderResponse>(command);
            AccommodationOrderEntity order = new AccommodationOrderEntity(response.newOrderId, command.CustomerId, command.Requirement, DateTime.Now, null, null);
            await orderRepository.AddAsync(order);
            return new CreateAccommodationOrderResponse(order.Id);
        }
    }
}