using Kontrer.Shared.Models.Pricing.Blueprints;

namespace Kontrer.OwnerServer.PricingService.Business.Pricing.PricingMiddlewares
{
    public interface IAccommodationPricer : IPricer<AccommodationBlueprint,RawAccommodationCost>
    {
        
    }
}