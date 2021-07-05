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
    }

    //public class CancelAccommodationOrderCommand : ICommand
    //{
    //    public CancelAccommodationOrderCommand(int orderId, string reasonMessage, bool isCanceledByCustomer)
    //    {
    //        OrderId = orderId;
    //        ReasonMessage = reasonMessage;
    //        IsCanceledByCustomer = isCanceledByCustomer;
    //    }

    //    public int OrderId { get; set; }
    //    public string ReasonMessage { get; set; }
    //    public bool IsCanceledByCustomer { get; set; }
    //}
}