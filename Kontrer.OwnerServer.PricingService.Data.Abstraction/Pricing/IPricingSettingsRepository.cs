using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;

namespace Kontrer.OwnerServer.PricingService.Data.Abstraction.Pricing
{
    public interface IPricingSettingsRepository : ITrackingRepository
    {      
        void AddTimedSetting(TimedSettingModel timedSetting);
        void EditTimedSetting(TimedSettingModel timedSetting);
        void RemoveTimedSetting(int settingGroupId, DateTime start, DateTime end);
        Task<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>> GetTimedSettingsAsync(List<TimedSettingSelector> selectors);
        Task<NullableResult<TSetting>> GetTimedSetting<TSetting>(TimedSettingSelector selector);
    }
}
