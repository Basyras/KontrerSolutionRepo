using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Settings
{
    public class SettingNameConstants
    {
        public const string CustomerPercentageDiscountPerAccommodation = "CustomerPercentageDiscountPerAccommodation";
        public readonly static SettingRequest<float> CustomerPercentageDiscountPerAccommodationRequest = new SettingRequest<float>(CustomerPercentageDiscountPerAccommodation);

        public const string MaxCustomerPercentageDiscountPerAccommodation = "MaxCustomerPercentageDiscountPerAccommodation";
        public readonly static SettingRequest<float> MaxCustomerPercentageDiscountPerAccommodationRequest = new SettingRequest<float>(MaxCustomerPercentageDiscountPerAccommodation);        
    }
}
