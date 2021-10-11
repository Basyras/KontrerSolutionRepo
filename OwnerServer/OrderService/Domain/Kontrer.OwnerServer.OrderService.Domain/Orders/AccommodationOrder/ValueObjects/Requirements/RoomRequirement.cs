using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder.ValueObjects.Requirements
{
    public class RoomRequirement
    {
        public DateTime RoomStartDate { get; set; }
        public DateTime RoomEndDate { get; set; }
        public List<ItemRequirement> RoomItems { get; set; } = new List<ItemRequirement>();
        public List<PersonRequirement> People { get; set; } = new List<PersonRequirement>();
        public List<DiscountBlueprint> Discounts { get; set; } = new List<DiscountBlueprint>();
        public string RoomType { get; set; }

        public static readonly IReadOnlyList<string> RoomTypes = new List<string>()
        {
            "standard",
            "economy",
            "luxury"
        };
    }
}