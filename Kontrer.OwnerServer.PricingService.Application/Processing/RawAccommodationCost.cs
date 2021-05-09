using Kontrer.Shared.Models.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.PricingService.Application.Processing
{
    public class RawAccommodationCost
    {
        public RawAccommodationCost(Currencies currency, List<RawItemCost> rawAccommodationItems, List<RawRoomCost> rawRooms)
        {
            RawAccommodationItems = rawAccommodationItems;
            RawRooms = rawRooms;
            Currency = currency;
            
        }

        public List<RawItemCost> RawAccommodationItems { get; }
        public List<RawRoomCost> RawRooms { get; }
        public Currencies Currency { get; }

    }
}