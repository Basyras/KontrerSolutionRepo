using Kontrer.OwnerServer.PricingService.Infrastructure.Abstraction.Pricing;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.PricingService.Application.Pricing.Pricers
{

    public interface IPricer<TBlueprint,TCost> : IPriceManipulationDescription
    {              
        void CalculateContractCost(TBlueprint blueprint, TCost rawAccommodation, ITimedSettingResolver resolver);
        List<TimedSettingSelector> GetRequiredSettings(TBlueprint blueprint);
    }

  
}