using Kontrer.Shared.Models.Pricing.Blueprints;

namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    public interface IAccommodationPricer : IPricer<AccommodationBlueprint,RawAccommodationCost>
    {
        
    }
}