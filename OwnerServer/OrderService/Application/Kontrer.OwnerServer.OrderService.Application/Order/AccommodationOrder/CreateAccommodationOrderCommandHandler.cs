using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.OwnerServer.OrderService.Dtos.Models;
using Kontrer.Shared.DomainDrivenDesign.Application;
using Kontrer.Shared.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder
{
    public class CreateAccommodationOrderCommandHandler : ICommandHandler<CreateAccommodationOrderCommand, CreateAccommodationOrderResponse>
    {
        private readonly IAccommodationOrderRepository orderRepository;
        private readonly IMessageBusManager messageBus;

        public CreateAccommodationOrderCommandHandler(IAccommodationOrderRepository orderRepository, IMessageBusManager messageBus)
        {
            this.orderRepository = orderRepository;
            this.messageBus = messageBus;
        }

        public async Task<CreateAccommodationOrderResponse> Handle(CreateAccommodationOrderCommand command, CancellationToken cancellationToken = default)
        {
            var response = await messageBus.RequestAsync<CreateAccommodationOrderCommand, CreateAccommodationOrderResponse>(command);
            await orderRepository.AddAsync(response.NewOrder);
            return new CreateAccommodationOrderResponse(response.NewOrder);
        }
    }
}