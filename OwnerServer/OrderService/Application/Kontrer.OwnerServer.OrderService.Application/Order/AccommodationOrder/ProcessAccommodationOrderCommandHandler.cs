using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.DomainDrivenDesign.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder
{
    public class ProcessAccommodationOrderCommandHandler : ICommandHandler<ProcessAccommodationOrderCommand>
    {
        private readonly IAccommodationOrderRepository repository;

        public ProcessAccommodationOrderCommandHandler(IAccommodationOrderRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(ProcessAccommodationOrderCommand request, CancellationToken cancellationToken = default)
        {
            request.Order.State = Domain.Orders.OrderStates.Processed;
            await repository.InstaUpdateAsync(request.Order);
        }
    }
}