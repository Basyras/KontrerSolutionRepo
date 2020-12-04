
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Abstraction.Pricing
{
    public interface IPricingManager
    {        

        Task<AccommodationCost> CalculateAccommodationCost(AccommodationBlueprint blueprint);
        IPricingSettingsUnitOfWork CreatePricingSettingsUnitOfWork();


    }
}
