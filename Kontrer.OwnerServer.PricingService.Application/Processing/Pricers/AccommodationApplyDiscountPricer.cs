using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.Pricers
{
    public class AccommodationApplyDiscountPricer : AccommodationPricerBase
    {
        public override int QueuePosition => int.MaxValue - 1000;

        public override string WorkDescription => "Applies discount before applying taxes";

        protected override void CallForEveryItem(ItemBlueprint blueprint, RawItemCost rawCost, IResolvedScopedSettings resolver)
        {
            foreach (var discount in blueprint.Discounts.Where(x => x.IsPercentageDiscount == false))
            {
                rawCost.ManipulateCost("Apply value discount", rawCost.SubTotal - discount.AmountDiscount.Amount);
            }

            foreach (var discount in blueprint.Discounts.Where(x=>x.IsPercentageDiscount))
            {
                rawCost.ManipulateCost("Apply percentage discount", rawCost.SubTotal * (decimal)discount.PercentageDiscount);
            }

            base.CallForEveryItem(blueprint, rawCost, resolver);
        }


    }
}
