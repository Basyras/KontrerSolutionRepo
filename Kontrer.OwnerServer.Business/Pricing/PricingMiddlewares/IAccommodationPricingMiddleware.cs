using Kontrer.Shared.Models.Pricing.Blueprints;

namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    public interface IAccommodationPricingMiddleware : IPricingMiddleware<AccommodationBlueprint,RawAccommodationCost>
    {
        
    }
}