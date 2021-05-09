using Kontrer.Shared.Models.Pricing.Blueprints;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.Pricers
{
    public interface IAccommodationPricer : IPricer<AccommodationBlueprint,RawAccommodationCost>
    {
        
    }
}