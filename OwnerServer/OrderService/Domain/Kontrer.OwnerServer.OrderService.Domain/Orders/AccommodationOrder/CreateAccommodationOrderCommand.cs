using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder
{
    public record CreateAccommodationOrderCommand(int CustomerId, AccommodationRequirement Requirement) : ICommand<CreateAccommodationOrderResponse>;
}