using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;

namespace Kontrer.OwnerServer.Data.Abstraction.Pricing
{
    public interface IPricingSettingsRepository : IRepository
    {
        //void AddSettingGroup();
        //void RemoveSettingGroup();
        void AddTimedSetting(TimedSetting timedSetting);
        void EditTimedSetting(TimedSettingSelector selector, TimedSetting timedSetting);
        void RemoveTimedSetting(TimedSettingSelector request);
        void GetTimedSettings(List<TimedSettingSelector> requests);
        Task<NullableResult<TSetting>> GetTimedSetting<TSetting>(TimedSettingSelector request);
    }
}
