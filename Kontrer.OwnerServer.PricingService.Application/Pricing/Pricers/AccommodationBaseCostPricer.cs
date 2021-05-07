using Kontrer.OwnerServer.PricingService.Infrastructure.Abstraction.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Pricing.Pricers
{
    public class AccommodationBaseCostPricer : AccommodationPricerBase
    {
        public override int QueuePosition => 1;
        public override string WorkDescription => "Adding items base costs";

        public override List<TimedSettingSelector> GetRequiredSettings(AccommodationBlueprint blueprint)
        {
            return null;
        }

        protected override void CallForEveryItem(ItemBlueprint blueprint, RawItemCost rawCost, ITimedSettingResolver resolver)
        {        

            var newSubTotal = rawCost.SubTotal +  blueprint.Count * blueprint.CostPerOne.Amount;
            rawCost.ManipulateCost(this, newSubTotal);
            base.CallForEveryItem(blueprint, rawCost,resolver);
        }

     


    }
}
