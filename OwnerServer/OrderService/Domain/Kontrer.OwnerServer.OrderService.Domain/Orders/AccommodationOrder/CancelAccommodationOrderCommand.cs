using Basyc.DomainDrivenDesign.Domain;
using Kontrer.OwnerServer.OrderService.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder
{
    public record CancelAccommodationOrderCommand(int OrderId, string ReasonMessage, bool IsCanceledByCustomer) : ICommand;

    //public record CancelAccommodationOrderCommand(OrderCustomer Customer, int OrderId, string ReasonMessage, bool IsCanceledByCustomer)
    //    : CommandWithContextBase<OrderCustomer>(Customer);
}