using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Client.Models.Blueprints
{
    public class RoomBlueprint
    {
        public DateTime RoomStartDate { get; set; }
        public DateTime RoomEndDate { get; set; }
        public List<ItemBlueprint> RoomItems { get; set; }
        public List<PersonBlueprint> People { get; set; }
        public List<DiscountBlueprint> Discounts { get; set; }
        public string RoomType { get; set; }

        public static readonly IReadOnlyList<string> RoomTypes = new List<string>()
        {
            "standard",
            "economy",
            "luxury"
        };
    }
}