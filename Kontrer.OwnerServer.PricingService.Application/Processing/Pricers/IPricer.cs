using Kontrer.OwnerServer.PricingService.Application.Settings;
using System.Collections.Generic;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.Pricers
{

    public interface IPricer<TBlueprint,TCost> : IPriceManipulationDescription
    {              
        void CalculateContractCost(TBlueprint blueprint, TCost rawAccommodation, IScopedSettings settings);
        List<SettingRequest> GetRequiredSettings(TBlueprint blueprint);
    }

  
}