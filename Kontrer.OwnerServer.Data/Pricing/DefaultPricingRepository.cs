using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Pricing
{
    public class DefaultPricingSettingsRepository : IPricingSettingsRepository
    {
        public void AddTimedSetting(TimedSetting timedSetting)
        {
            throw new NotImplementedException();
        }

        public void EditTimedSetting(TimedSettingSelector selector, TimedSetting timedSetting)
        {
            throw new NotImplementedException();
        }

        public Task<NullableResult<TSetting>> GetTimedSetting<TSetting>(TimedSettingSelector request)
        {
            throw new NotImplementedException();
        }

        public void GetTimedSettings(List<TimedSettingSelector> requests)
        {
            throw new NotImplementedException();
        }

        public void RemoveTimedSetting(TimedSettingSelector request)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
