using Kontrer.OwnerServer.Business.Pricing.BlueprintEditors;
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
        public void EditBlueprint(AccommodationBlueprint blueprint, IPricingSettingsResolver resolver)
        {

            DiscountBlueprint discount = resolver.ResolveSettingValue<DiscountBlueprint>(SettingNameConstants.CustomerLoayltyDiscount,blueprint.Start, blueprint.End);
            blueprint.Discounts.Add(discount);

        }
    }
}
