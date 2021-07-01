using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.OwnerServer.Shared.MessageBus;
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

        public override async Task Handle(CancelAccommodationOrderCommand command, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.GetAsync(command.OrderId);
            order.State = command.IsCanceledByCustomer ? OrderStates.CanceledByCustomer : OrderStates.CanceledByOwner;
            await orderRepository.UpdateAsync(order);

            if (command.IsCanceledByCustomer == false)
            {
                order.State = OrderStates.CanceledByOwner;
                //should notify customer
            }
        }
    }
}