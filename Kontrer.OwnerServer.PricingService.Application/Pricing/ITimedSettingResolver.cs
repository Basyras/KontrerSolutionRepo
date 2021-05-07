using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using System;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Pricing
{
    /// <summary>
    /// Returns null when not found. Use T? for structs
    /// </summary>
    #nullable enable
    public interface ITimedSettingResolver
    {
        //TSetting ResolveSettingValue<TSetting>(string settingUniqueName);
        //NullableResult<TSetting> ResolveValue<TSetting>(string settingUniqueName);        
        NullableResult<TSetting> ResolveValue<TSetting>(ResolveRequest<TSetting> request);        
    }
}