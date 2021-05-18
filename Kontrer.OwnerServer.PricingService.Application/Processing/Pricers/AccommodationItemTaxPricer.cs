using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.Pricers
{
    /// <summary>
    /// Must be called as last in pricing pipeline
    /// </summary>
    public class AccommodationItemTaxPricer : AccommodationPricerBase
    {
        public override string WorkDescription => "Adding taxes from ItemBlueprint as last in queue";
        public override int QueuePosition => int.MaxValue;

        protected override void CallForEveryItem(ItemBlueprint blueprint, RawItemCost rawCost, IResolvedScopedSettings settings)
        {
            decimal taxAmountToAdd = rawCost.SubTotal * (decimal)blueprint.TaxPercentageToAdd;
            decimal newSubTotal = rawCost.SubTotal + taxAmountToAdd;
            rawCost.ManipulateCost(this, newSubTotal);
            base.CallForEveryItem(blueprint, rawCost,settings);
        }
    }
}
