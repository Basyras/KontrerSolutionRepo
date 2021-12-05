using Basyc.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder
{
    public record ProcessAccommodationOrderCommand(AccommodationOrderEntity Order) : ICommand;
}