using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.Pricers
{
    public interface IAccommodationPricer : IPricer<AccommodationBlueprint,RawAccommodationCost>
    {
        
    }
}