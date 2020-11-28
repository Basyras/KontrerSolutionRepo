using Kontrer.Shared.Models.Pricing;
using System;

namespace Kontrer.OwnerServer.Business.Pricing
{
    public interface IPricingSettingsResolver
    {
        //TSetting ResolveSettingValue<TSetting>(string settingUniqueName);
        TSetting ResolveSettingValue<TSetting>(string settingUniqueName, DateTime? start = null, DateTime? end = null);

        bool TryResolveSettingValue<TSetting>(string settingUniqueName, out TSetting setting, DateTime? start = null, DateTime? end = null);
    }
}