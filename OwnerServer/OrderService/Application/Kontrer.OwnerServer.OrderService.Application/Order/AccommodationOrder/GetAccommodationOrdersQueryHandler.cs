using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder
{
    public class GetAccommodationOrdersQueryHandler : IQueryHandler<GetAccommodationOrdersQuery, GetAccommodationOrdersResponse>
    {
        private readonly IAccommodationOrderRepository repository;

        public GetAccommodationOrdersQueryHandler(IAccommodationOrderRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GetAccommodationOrdersResponse> Handle(GetAccommodationOrdersQuery command, CancellationToken cancellationToken = default)
        {
            var orders = await repository.GetAllAsync();
            var response = new GetAccommodationOrdersResponse(orders);
            return response;
        }
    }
}