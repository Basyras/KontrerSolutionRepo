using Kontrer.Shared.Models.Pricing.Blueprints;

namespace Kontrer.OwnerServer.PricingService.Application.Pricing.Pricers
{
    public interface IAccommodationPricer : IPricer<AccommodationBlueprint,RawAccommodationCost>
    {
        
    }
}