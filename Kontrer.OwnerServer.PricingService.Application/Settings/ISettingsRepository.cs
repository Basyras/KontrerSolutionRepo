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
        void AddScopedSetting<TSetting>(string settingName, DateTime from, DateTime to, TSetting value);
        void RemoveScopedSetting(string settingName, DateTime from, DateTime to);
        void UpdateScopedSetting<TSetting>(string settingName, DateTime from, DateTime to, TSetting value);

        Task<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>> GetAllScopedSettingsAsync();
        Task<NullableResult<TSetting>> GetScopedSettingAsync<TSetting>(DateTime from, DateTime to, SettingRequest<TSetting> request);
        /// <summary>
        /// Returnes settings with best matching scope, value is of tybe object but should match request type!!
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="requests"></param>
        /// <returns></returns>
        Task<IDictionary<string, NullableResult<object>>> GetScopedSettingsAsync(DateTime from, DateTime to, List<SettingRequest> requests);


        void AddTimeScope(string scopeName, DateTime from, DateTime to);
        void RemoveTimeScope(int scopeId);
        void UpdateTimeScope(TimeScope scope);
        Task<TimeScope> GetTimeScope(int scopeId);
        Task<List<TimeScope>> GetTimeScopes();
        

       
    }
}
