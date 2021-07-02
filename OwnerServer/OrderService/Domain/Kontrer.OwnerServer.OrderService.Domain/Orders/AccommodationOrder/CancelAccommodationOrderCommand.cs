using Kontrer.Shared.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder
{
    public record CancelAccommodationOrderCommand(int OrderId, string ReasonMessage, bool IsCanceledByCustomer) : ICommand
    {
        //public CancelAccommodationOrderCommand() : this(99, "99", false)
        //{
        //}
    }
}