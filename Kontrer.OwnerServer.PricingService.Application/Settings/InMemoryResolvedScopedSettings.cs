using Basyc.Shared.Models;
using Kontrer.OwnerServer.PricingService.Application.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Settings
{

    public class InMemoryResolvedScopedSettings : IResolvedScopedSettings
    {        
        //private readonly IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>> CachedSettings;
        private readonly IDictionary<string, NullableResult<object>> CachedSettings;

        public InMemoryResolvedScopedSettings(DateTime timeFrom, DateTime timeTo, IDictionary<string, NullableResult<object>> settings)
        {
            TimeFrom = timeFrom;
            TimeTo = timeTo;
            this.CachedSettings = settings;
        }

        public DateTime TimeFrom { get; }
        public DateTime TimeTo { get; }

        public NullableResult<TSetting> GetSetting<TSetting>(SettingRequest<TSetting> request)
        {
            NullableResult<object> setting = CachedSettings[request.UniqueSettingName];
            return new NullableResult<TSetting>((TSetting)setting.Value,setting.WasFound, (TSetting)setting.DefaultValue);
        }
    }
}
