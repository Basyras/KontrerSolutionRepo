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

        public List<RoomRequirement> Rooms { get; set; } = new List<RoomRequirement>();
    }
}