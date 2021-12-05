using Basyc.DomainDrivenDesign.Application;
using Basyc.MessageBus;
using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Customers;
using Kontrer.OwnerServer.OrderService.Domain.Orders;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
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
        private readonly IMessageBusClient messageBus;

        public CancelAccommodationOrderCommandHandler(IAccommodationOrderRepository orderRepository, IMessageBusClient messageBus)
        {
            this.orderRepository = orderRepository;
            this.messageBus = messageBus;
        }

        public async Task Handle(CancelAccommodationOrderCommand command, CancellationToken cancellationToken = default)
        {
            // var hashCode = command.Context.Id.GetHashCode();
            if (new Random().Next(0, 1) == 1)
            {
                throw new Exception("Not Authorized");
            }

            var order = await orderRepository.TryGetAsync(command.OrderId);
            cancellationToken.ThrowIfCancellationRequested();

            if (order == null)
            {
                throw new InvalidOperationException($"Could not cancel order with id {command.OrderId} does not exist");
            }
            order.State = command.IsCanceledByCustomer ? OrderStates.CanceledByCustomer : OrderStates.CanceledByOwner;
            await orderRepository.InstaUpdateAsync(order);

            cancellationToken.ThrowIfCancellationRequested();

            if (command.IsCanceledByCustomer == false)
            {
                //should notify customer
            }
        }
    }
}