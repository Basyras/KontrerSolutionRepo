using Kontrer.OwnerServer.Business.Pricing.BlueprintEditors;
using Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public class PriceManagerOptions
    {
        public List<IAccommodationPricingMiddleware> AccommodationPricers { get; set; } = new List<IAccommodationPricingMiddleware>();
        public List<IAccommodationBlueprintEditor> AccommodationEditors { get; set; } = new List<IAccommodationBlueprintEditor>();
        
    }
}