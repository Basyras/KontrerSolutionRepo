using Basyc.Shared.Models;
using Kontrer.OwnerServer.PricingService.Application.Processing;
using Kontrer.Shared.Models.Pricing;
using System;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Settings
{
    /// <summary>
    /// Returns cached settings for calculating the cost with defined time range. Returns null when not found. Use T? for structs
    /// </summary>
    //#nullable enable
    public interface IResolvedScopedSettings
    {        
        NullableResult<TSetting> GetSetting<TSetting>(SettingRequest<TSetting> request);        
        DateTime TimeFrom { get; }
        DateTime TimeTo { get; }
    }
}