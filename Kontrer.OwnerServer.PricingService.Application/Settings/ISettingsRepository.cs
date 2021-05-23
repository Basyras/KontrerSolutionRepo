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
        //Settings
        Task<Dictionary<string, Type>> GetSettingsAsync();
        void CreateNewSetting<TSetting>(string settingId);
        void CreateNewSetting(string settingId, Type settingType);
        void RemoveSetting(string settindId);

        //Scoped settings
        void AddScopedSetting<TSetting>(string settingId, int timeScopeId, TSetting value);
        void AddScopedSetting(string settingId, int timeScopeId, object value, Type settingType);
        void RemoveScopedSetting(string settingId, int timeScopeId);
        void UpdateScopedSetting(string settingId, int timeScopeId, object value, Type settingType);

        Task<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>> GetAllScopedSettingsAsync();
        Task<NullableResult<TSetting>> GetScopedSettingAsync<TSetting>(ScopedSettingRequest<TSetting> request);
        /// <summary>
        /// Returnes settings with best matching scope, value is of tybe object but should match request type!!
        /// </summary>      
        Task<IDictionary<string, NullableResult<object>>> GetScopedSettingsAsync(IEnumerable<ScopedSettingRequest> requests);

        //TimeScopes
        void CreateNewTimeScope(string scopeName, DateTime from, DateTime to);
        void RemoveTimeScope(int scopeId);
        void UpdateTimeScope(TimeScope scope);
        Task<TimeScope> GetTimeScope(int scopeId);
        Task<List<TimeScope>> GetTimeScopesAsync();
        
    }
}
