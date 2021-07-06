using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements
{
    public class AccommodationRequirement
    {
        public AccommodationRequirement()
        {
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public List<RoomRequirement> Rooms { get; set; } = new List<RoomRequirement>();
        public List<ItemRequirement> AccommodationItems { get; set; } = new List<ItemRequirement>();

        public List<DiscountBlueprint> Discounts { get; set; } = new List<DiscountBlueprint>();
    }
}