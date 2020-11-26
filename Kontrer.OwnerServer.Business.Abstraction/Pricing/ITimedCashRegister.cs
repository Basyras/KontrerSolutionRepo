using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Abstraction.Pricing
{
    public interface ITimedCashRegister
    {
        TimedZone TimeZone { get; }
        AccommodationCost GetContractCost(AccommodationBlueprint blueprint);
        Task<AccommodationCost> GetContractCostAsync(AccommodationBlueprint blueprint);
    }
}
