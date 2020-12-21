using Kontrer.OwnerServer.Business.Pricing.BlueprintEditors;
using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.Shared.Models;
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

            var accommodationCount = blueprint.Customer.Accomodations.Count(x => x.State == Shared.Models.AccommodationState.Completed);
            NullableResult<float> loyaltyPercentagePerAcco = resolver.ResolveValue(SettingNameConstants.CustomerPercentageDiscountPerAccommodationRequest);
            NullableResult<float> maxLoyaltyPercentage = resolver.ResolveValue(SettingNameConstants.MaxCustomerPercentageDiscountPerAccommodationRequest);
            loyaltyPercentagePerAcco.Value *= accommodationCount;
            loyaltyPercentagePerAcco = (loyaltyPercentagePerAcco.Value > maxLoyaltyPercentage.Value) ? maxLoyaltyPercentage : loyaltyPercentagePerAcco;
            DiscountBlueprint discount = new DiscountBlueprint("Customer loyality discount", loyaltyPercentagePerAcco.Value);
            blueprint.Discounts.Add(discount);
        }

        public List<TimedSettingSelector> GetRequiredSettings(AccommodationBlueprint blueprint)
        {
            return new List<TimedSettingSelector>()
            {
                new TimedSettingSelector(SettingNameConstants.CustomerPercentageDiscountPerAccommodation), 
                new TimedSettingSelector(SettingNameConstants.MaxCustomerPercentageDiscountPerAccommodation) 
            };
        }
    }
}
