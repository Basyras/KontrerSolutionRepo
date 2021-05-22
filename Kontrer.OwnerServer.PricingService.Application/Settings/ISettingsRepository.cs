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
        void AddSetting<TSetting>(string settingId);
        void RemoveSetting(string settindId);

        void AddScopedSetting<TSetting>(string settingId, int timeScopeId, TSetting value);
        void RemoveScopedSetting(string settingId, int timeScopeId);
        void UpdateScopedSetting<TSetting>(string settingId, int timeScopeId, TSetting value);

        Task<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>> GetAllScopedSettingsAsync();
        Task<NullableResult<TSetting>> GetScopedSettingAsync<TSetting>(ScopedSettingRequest<TSetting> request);
        /// <summary>
        /// Returnes settings with best matching scope, value is of tybe object but should match request type!!
        /// </summary>      
        Task<IDictionary<string, NullableResult<object>>> GetScopedSettingsAsync(IEnumerable<ScopedSettingRequest> requests);


        void AddTimeScope(string scopeName, DateTime from, DateTime to);
        void RemoveTimeScope(int scopeId);
        void UpdateTimeScope(TimeScope scope);
        Task<TimeScope> GetTimeScope(int scopeId);
        Task<List<TimeScope>> GetTimeScopesAsync();
        
    }
}
