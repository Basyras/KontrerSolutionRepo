using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kontrer.Shared.Models.Pricing.Costs
{
    public record AccommodationCost(
        ReadOnlyCollection<RoomCost> Rooms,
        ReadOnlyCollection<ItemCost> AccomodationItems, 
        Cash TotalCost);
}
