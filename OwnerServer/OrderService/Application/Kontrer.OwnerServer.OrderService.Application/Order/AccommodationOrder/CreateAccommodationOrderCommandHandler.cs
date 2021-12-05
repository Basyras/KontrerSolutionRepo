using Basyc.DomainDrivenDesign.Application;
using Basyc.MessageBus;
using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.OwnerServer.OrderService.Dtos.Models;
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
            var newOrder = new AccommodationOrderEntity(default, command.CustomerId, command.Requirement, DateTime.Now, "sheesh", "sheesh2");
            newOrder.State = Domain.Orders.OrderStates.New;
            newOrder = await orderRepository.InstaAddAsync(newOrder);
            return new CreateAccommodationOrderResponse(newOrder);
        }
    }
}