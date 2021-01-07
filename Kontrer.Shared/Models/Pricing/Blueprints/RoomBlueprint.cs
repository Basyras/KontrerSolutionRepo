using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models.Pricing.Blueprints
{
    public class RoomBlueprint
    {
        public DateTime RoomStartDate { get; set; }
        public DateTime RoomEndDate { get; set; }
        public List<ItemBlueprint> RoomItems {get; set;}
        public List<PersonBlueprint> People { get; set; }
        public List<DiscountBlueprint> Discounts { get; set; }
        public string RoomType { get; set; }


    }
}
