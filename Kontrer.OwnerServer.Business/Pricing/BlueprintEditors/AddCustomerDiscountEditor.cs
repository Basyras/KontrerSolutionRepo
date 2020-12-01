﻿using Kontrer.OwnerServer.Business.Pricing.BlueprintEditors;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing.BlueprintEditors
{
    public class AddCustomerDiscountEditor : IAccommodationBlueprintEditor
    {
        public void EditBlueprint(AccommodationBlueprint blueprint, ITimedSettingResolver resolver)
        {
            var discountResult = resolver.ResolveValue<DiscountBlueprint>(SettingNameConstants.CustomerLoayltyDiscount);
            blueprint.Discounts.Add(discountResult.Value);
        }

        public List<TimedSettingSelector> GetRequiredSettings(AccommodationBlueprint blueprint)
        {
            throw new NotImplementedException();
        }
    }
}