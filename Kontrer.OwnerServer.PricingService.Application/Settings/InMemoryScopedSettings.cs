using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Settings
{

    public class InMemoryScopedSettings : IScopedSettings
    {        
        //private readonly IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>> CachedSettings;
        private readonly IDictionary<string, NullableResult<object>> CachedSettings;

        public InMemoryScopedSettings(DateTime timeFrom, DateTime timeTo, IDictionary<string, NullableResult<object>> settings)
        {
            TimeFrom = timeFrom;
            TimeTo = timeTo;
            this.CachedSettings = settings;
        }

        public DateTime TimeFrom { get; }
        public DateTime TimeTo { get; }

        public NullableResult<TSetting> GetSetting<TSetting>(SettingRequest<TSetting> request)
        {
            NullableResult<object> setting;
            //if (request.Start.HasValue && request.End.HasValue)
            //{
            //    setting = CachedSettings[request.UniqueSettingName][new Tuple<DateTime, DateTime>(request.Start.Value, request.End.Value)];
            //}
            //else
            //{
            //    setting = CachedSettings[request.UniqueSettingName][new Tuple<DateTime, DateTime>(default, default)];
            //}

            //if(setting.WasFound == false)
            //{
            //    setting = CachedSettings[request.UniqueSettingName].First().Value;
            //}

            setting = CachedSettings[request.UniqueSettingName];
            return new NullableResult<TSetting>((TSetting)setting.Value,setting.WasFound, (TSetting)setting.DefaultValue);
        }
    }
}
