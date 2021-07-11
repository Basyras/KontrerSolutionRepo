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
    public class DeleteAccommodationOrderCommandHandler : ICommandHandler<DeleteAccommodationOrderCommand>
    {
        private readonly IAccommodationOrderRepository repo;

        public DeleteAccommodationOrderCommandHandler(IAccommodationOrderRepository repo)
        {
            this.repo = repo;
        }

        public async Task Handle(DeleteAccommodationOrderCommand request, CancellationToken cancellationToken = default)
        {
            await repo.RemoveAsync(request.OrderId);
        }
    }
}