using Kontrer.OwnerServer.OrderService.Application.Interfaces;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.DomainDrivenDesign.Application;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder
{
    public class GetNewAccommodationOrdersQueryHandler : QueryHandlerBase<GetNewAccommodationOrdersQuery, GetNewAccommodationOrdersResponse>
    {
        private readonly IAccommodationOrderRepository repository;

        public GetNewAccommodationOrdersQueryHandler(IAccommodationOrderRepository repository)
        {
            this.repository = repository;
        }

        public override async Task<GetNewAccommodationOrdersResponse> Handle(GetNewAccommodationOrdersQuery command, CancellationToken cancellationToken = default)
        {
            var orders = await repository.GetNewOrdersAsync();
            var response = new GetNewAccommodationOrdersResponse(orders);
            return response;
        }
    }
}