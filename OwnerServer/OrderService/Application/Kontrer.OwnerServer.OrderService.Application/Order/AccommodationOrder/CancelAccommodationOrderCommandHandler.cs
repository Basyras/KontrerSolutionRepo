using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.Shared.DomainDrivenDesign.Application;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder
{
    public class CancelAccommodationOrderCommandHandler : CommandHandlerBase<CancelAccommodationOrderCommand>
    {
        private readonly IAccommodationOrderRepository orderRepository;
        private readonly IMessageBusManager messageBus;

        public CancelAccommodationOrderCommandHandler(IAccommodationOrderRepository orderRepository, IMessageBusManager messageBus)
        {
            this.orderRepository = orderRepository;
            this.messageBus = messageBus;
        }

        public override Task Handle(CancelAccommodationOrderCommand command, CancellationToken cancellationToken = default)
        {
            return orderRepository.RemoveAsync(command.OrderId);
        }
    }
}