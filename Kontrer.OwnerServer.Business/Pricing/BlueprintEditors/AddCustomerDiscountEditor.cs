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
            var discountResult = resolver.ResolveValue<DiscountBlueprint>(SettingNameConstants.CustomerLoayltyDiscount);
            blueprint.Discounts.Add(discountResult.Value);
        }

        public List<SettingRequest> GetRequiredSettings(AccommodationBlueprint blueprint)
        {
            throw new NotImplementedException();
        }
    }
}
