using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    /// <summary>
    /// Must be called as last in pricing pipeline
    /// </summary>
    public class ItemTaxMiddleware : AccommodationPricingMiddlewareBase
    {
        public override string WorkDescription => "Adding taxes from ItemBlueprint";
        public override int QueuePosition => int.MaxValue;
     
        protected override void CallForEveryItem(ItemBlueprint blueprint, RawItemCost rawCost, ITimedSettingResolver resolver)
        {
            decimal taxAmountToAdd = rawCost.SubTotal * (decimal)blueprint.TaxPercentageToAdd;
            decimal newSubTotal = rawCost.SubTotal + taxAmountToAdd;
            rawCost.ManipulateCost(this, newSubTotal);
            base.CallForEveryItem(blueprint, rawCost,resolver);
        }
    }
}
