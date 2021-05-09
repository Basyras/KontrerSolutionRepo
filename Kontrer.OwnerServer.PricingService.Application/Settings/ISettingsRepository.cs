using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Settings
{
    public interface ISettingsRepository : IBulkRepository
    {
        void RemoveScopedSetting(string settingUniqueName, DateTime start, DateTime end);
        void AddScopedSetting<TSetting>(string settingUniqueName, DateTime start, DateTime end, TSetting value);
        void RemoveDefaultScopedSetting(string settingUniqueName);
        void AddDefaultScopedSetting<TSetting>(string settingUniqueName, TSetting value);

        void UpdateScopedSettings<TSetting>(string settingUniqueName, TSetting value);


        void RemoveTimeScope(int scopeId);
        void AddTimeScope(string scopeName, DateTime from, DateTime to);
        void UpdateTimeScope(TimeScope scope);

        Task<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>> GetAllScopedSettingsAsync();
        Task<IDictionary<string, NullableResult<object>>> GetScopedSettingsAsync(DateTime from, DateTime to,List<SettingRequest> requests);
        Task<NullableResult<TSetting>> GetScopedSettingAsync<TSetting>(DateTime from, DateTime to, SettingRequest<TSetting> request);
    }
}
