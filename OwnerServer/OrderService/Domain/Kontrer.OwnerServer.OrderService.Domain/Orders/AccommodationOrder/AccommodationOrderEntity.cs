using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder
{
    public class AccommodationOrderEntity : OrderEntityBase<AccommodationRequirement>
    {
        public AccommodationOrderEntity()
        {
        }

        public AccommodationOrderEntity(int id, int customerId, AccommodationRequirement requirment, DateTime issueDate, string customerNotes, string ownerPrivateNotes)
            : base(id, customerId, requirment, issueDate, customerNotes, ownerPrivateNotes)
        {
        }
    }
}