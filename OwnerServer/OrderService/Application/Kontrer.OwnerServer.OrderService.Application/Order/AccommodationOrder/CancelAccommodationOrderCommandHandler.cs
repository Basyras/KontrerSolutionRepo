using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.DomainDrivenDesign.Application;
using Kontrer.Shared.MessageBus;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder
{
    public class CancelAccommodationOrderCommandHandler : ICommandHandler<CancelAccommodationOrderCommand>
    {
        private readonly IAccommodationOrderRepository orderRepository;
        private readonly IMessageBusManager messageBus;

        public CancelAccommodationOrderCommandHandler(IAccommodationOrderRepository orderRepository, IMessageBusManager messageBus)
        {
            this.orderRepository = orderRepository;
            this.messageBus = messageBus;
        }

        public async Task Handle(CancelAccommodationOrderCommand command, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.TryGetAsync(command.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Could not remove order with id: {command.OrderId}. Order does not exist");
            }
            order.State = command.IsCanceledByCustomer ? OrderStates.CanceledByCustomer : OrderStates.CanceledByOwner;
            await orderRepository.InstaUpdateAsync(order);

            if (command.IsCanceledByCustomer == false)
            {
                //should notify customer
            }
        }
    }
}