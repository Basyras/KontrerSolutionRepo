using Kontrer.Shared.Models.Pricing;
using System;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public interface IPricingSettingsResolver
    {
        TSetting ResolveSettingValue<TSetting>(DateTime? start, DateTime? end);
    }
}