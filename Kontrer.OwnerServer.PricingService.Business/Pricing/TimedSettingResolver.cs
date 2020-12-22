using Kontrer.OwnerServer.PricingService.Business.Abstraction.Pricing;
using Kontrer.OwnerServer.PricingService.Data.Abstraction.Pricing;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Business.Pricing
{

    public class TimedSettingResolver : ITimedSettingResolver
    {
        private readonly IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>> ResolvedSettings;

        public TimedSettingResolver(IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>> resolvedSettings)
        {
            this.ResolvedSettings = resolvedSettings;
        }

     

        public NullableResult<TSetting> ResolveValue<TSetting>(ResolveRequest<TSetting> request)
        {
            NullableResult<object> setting;
            if (request.Start.HasValue && request.End.HasValue)
            {
                setting = ResolvedSettings[request.UniqueSettingName][new Tuple<DateTime, DateTime>(request.Start.Value, request.End.Value)];
            }
            else
            {
                setting = ResolvedSettings[request.UniqueSettingName].First().Value;
            }
            return new NullableResult<TSetting>((TSetting)setting.Value,setting.WasFound, (TSetting)setting.DefaultValue);
        }
    }
}
