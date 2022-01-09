﻿using Basyc.Shared.Models;
using Kontrer.OwnerServer.OrderService.Dtos.Models.Blueprints;
using Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Processing.BlueprintEditors
{
    public class AddCustomerHistoryDiscountEditor : IAccommodationBlueprintEditor
    {


        public void EditBlueprint(AccommodationBlueprint blueprint, IResolvedScopedSettings resolver)
        {

            //var accommodationCount = blueprint.CustomerId.Accomodations.Count();
            var accommodationCount = 0;
            throw new NotImplementedException();
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
