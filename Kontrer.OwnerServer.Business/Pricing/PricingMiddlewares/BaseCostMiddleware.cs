﻿using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing.PricingMiddlewares
{
    public class BaseCostMiddleware : AccommodationPricingMiddlewareBase
    {
        public override int QueuePosition => 1;
        public override string WorkDescription => "Calculating a basic cost by Count * CostPerOne";

        protected override void CallForEveryItem(ItemBlueprint blueprint, RawItemCost rawCost, ITimedSettingResolver resolver)
        {
         

            var newSubTotal = blueprint.Count * blueprint.CostPerOne.Amout;
            rawCost.ManipulateCost(this, newSubTotal);
            base.CallForEveryItem(blueprint, rawCost,resolver);
        }

     


    }
}
