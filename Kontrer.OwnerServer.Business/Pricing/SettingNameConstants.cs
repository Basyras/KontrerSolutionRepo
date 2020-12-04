using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public class SettingNameConstants
    {
        public const string CustomerPercentageDiscountPerAccommodation = "CustomerPercentageDiscountPerAccommodation";
        public readonly static ResolveRequest<float> CustomerPercentageDiscountPerAccommodationRequest = new ResolveRequest<float>(CustomerPercentageDiscountPerAccommodation);

        public const string MaxCustomerPercentageDiscountPerAccommodation = "MaxCustomerPercentageDiscountPerAccommodation";
        public readonly static ResolveRequest<float> MaxCustomerPercentageDiscountPerAccommodationRequest = new ResolveRequest<float>(MaxCustomerPercentageDiscountPerAccommodation);        
    }
}
