using Kontrer.Shared.Models.Pricing;
using System;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public interface IPricingSettingsResolver
    {
        TimedPricingSetting ResolveSetting(DateTime? start, DateTime? end);
    }
}