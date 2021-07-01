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
        public override Task<GetNewAccommodationOrdersResponse> Handle(GetNewAccommodationOrdersQuery command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}