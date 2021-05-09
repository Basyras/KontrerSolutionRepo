using Kontrer.Shared.Models.Pricing;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.PricingService.Application.Processing
{
    public class RawRoomCost
    {
        public RawRoomCost(List<RawItemCost> rawRoomItems, List<RawPersonCost> rawPeople)
        {
            RawRoomItems = rawRoomItems;
            RawPeople = rawPeople;
        }

        public List<RawItemCost> RawRoomItems { get; set; }
        public List<RawPersonCost> RawPeople { get; set; }
        public Cash Total { get; set; }
        
    }
}