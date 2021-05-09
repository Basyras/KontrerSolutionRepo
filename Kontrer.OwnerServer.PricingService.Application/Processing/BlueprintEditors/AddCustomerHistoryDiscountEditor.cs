using Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors
{
    public class AddCustomerHistoryDiscountEditor : IAccommodationBlueprintEditor
    {


        public void EditBlueprint(AccommodationBlueprint blueprint, IScopedSettings resolver)
        {

            var accommodationCount = blueprint.Customer.Accomodations.Count();
            NullableResult<float> loyaltyPercentagePerAcco = resolver.GetSetting(SettingNameConstants.CustomerPercentageDiscountPerAccommodationRequest);
            NullableResult<float> maxLoyaltyPercentage = resolver.GetSetting(SettingNameConstants.MaxCustomerPercentageDiscountPerAccommodationRequest);
            loyaltyPercentagePerAcco.Value *= accommodationCount;
            loyaltyPercentagePerAcco = (loyaltyPercentagePerAcco.Value > maxLoyaltyPercentage.Value) ? maxLoyaltyPercentage : loyaltyPercentagePerAcco;
            DiscountBlueprint discount = new DiscountBlueprint("Customer loyality discount", loyaltyPercentagePerAcco.Value);
            blueprint.Discounts.Add(discount);
        }

        public List<SettingRequest> GetRequiredSettings(AccommodationBlueprint blueprint)
        {
            return new List<SettingRequest>()
            {
                new SettingRequest<float>(SettingNameConstants.CustomerPercentageDiscountPerAccommodation), 
                new SettingRequest<float>(SettingNameConstants.MaxCustomerPercentageDiscountPerAccommodation) 
            };
        }
    }
}
