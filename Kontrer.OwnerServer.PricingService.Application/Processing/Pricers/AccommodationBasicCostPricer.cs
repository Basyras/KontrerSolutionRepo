using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.Pricers
{
    public class AccommodationBasicCostPricer : AccommodationPricerBase
    {
        public override int QueuePosition => 1;
        public override string WorkDescription => $"Adding items base costs. {nameof(RawItemCost.SubTotal)} + {nameof(ItemBlueprint.CostPerOne.Amount)} * {nameof(ItemBlueprint.Count)}";     

        protected override void CallForEveryItem(ItemBlueprint blueprint, RawItemCost rawCost, IResolvedScopedSettings resolver)
        {
            var newSubTotal = rawCost.SubTotal +  blueprint.Count * blueprint.CostPerOne.Amount;
            rawCost.ManipulateCost(this, newSubTotal);
            base.CallForEveryItem(blueprint, rawCost,resolver);
        }

     


    }
}
