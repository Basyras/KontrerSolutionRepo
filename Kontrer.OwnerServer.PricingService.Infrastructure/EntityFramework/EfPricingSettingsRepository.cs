using Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.PricingService.Application.Settings;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{

    public class EfPricingSettingsRepository : ISettingsRepository
    {
        private readonly PricingServiceDbContext dbContext;




        public EfPricingSettingsRepository(PricingServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddScopedSetting<TSetting>(string settingName, DateTime from, DateTime to, TSetting value)
        {
            dbContext.ScopedSettings.Add(new PricingScopedSettingEntity()
            {
                From = from,
                To = to,
                Value = value,
                Setting = new PricingSettingEntity() { PricingSettingEntityId = settingName }
            });
        }


        public void RemoveScopedSetting(string settingName, DateTime from, DateTime to)
        {
            PricingScopedSettingEntity entity = new PricingScopedSettingEntity()
            {
                PricingScopedSettingEntityId = settingName,
                From = from,
                To = to,
            };
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public void UpdateScopedSetting<TSetting>(string settingName, DateTime from, DateTime to, TSetting value)
        {
            PricingScopedSettingEntity newEntity = new PricingScopedSettingEntity();
            newEntity.PricingScopedSettingEntityId = settingName;
            newEntity.From = from;
            newEntity.To = to;
            newEntity.Value = value;

            var entityEntry = dbContext.Attach(newEntity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<NullableResult<TSetting>> GetScopedSettingAsync<TSetting>(DateTime from, DateTime to, SettingRequest<TSetting> request)
        {
            var settingEntity = await dbContext.Settings.AsQueryable()
                .Where(x => x.PricingSettingEntityId == request.UniqueSettingName)
                .SelectMany(x => x.ScopedSettings)
                .Where(x => x.From <= from && x.To >= to)
                .OrderBy(x => Math.Abs((x.From - from).TotalDays))
                .FirstAsync();

            if (settingEntity != null)
            {
                return new NullableResult<TSetting>((TSetting)settingEntity.Value, true, default);
            }
            else
            {
                return new NullableResult<TSetting>(default, false, default);
            }
        }

        public async Task<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>> GetAllScopedSettingsAsync()
        {
            var settingEntities = await dbContext.Settings
                .Select(settingEntity => new { Name = settingEntity.PricingSettingEntityId, Settings = settingEntity.ScopedSettings.ToDictionary(scopedEntity => new Tuple<DateTime, DateTime>(scopedEntity.From, scopedEntity.To), scopedEntity => new NullableResult<object>(scopedEntity.Value, true, default)) })
                .ToDictionaryAsync(dicPair => dicPair.Name, dicPair => dicPair.Settings);

            return settingEntities as IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>;
        }


        public async Task<IDictionary<string, NullableResult<object>>> GetScopedSettingsAsync(DateTime from, DateTime to, List<SettingRequest> requests)
        {
            //var query = dbContext.PricingTimedSettings.Where(x => selectors.Any(y => y.UniqueSettingName == x.PricingSettingGroup.SettingName && y.Start == x.Start && y.End == x.End));
            //var dic = await query.ToDictionaryAsync(x => x.PricingSettingGroup.SettingName, x => (IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>)x.PricingSettingGroup.TimedSettings.ToDictionary(y => new Tuple<DateTime, DateTime>(y.Start, y.End), y =>
            //      {
            //          return y == null ? new NullableResult<object>(default, false) : new NullableResult<object>(y.Value, true);
            //      }));

            //return dic;


            //var scopedSettingsEntities = new Dictionary<string, PricingScopedSettingEntity>();
            //var scopes = await dbContext.PricingSettingTimeScopes.Where(x => x.Start <= from && x.End >= to).ToListAsync();
            //foreach (var scopedSetting in scopes.SelectMany(x => x.ScopedSettings))
            //{
            //    if (scopedSettingsEntities.TryGetValue(scopedSetting.PricingSettingGroup.SettingName, out PricingScopedSettingEntity foundSetting))
            //    {
            //        if (foundSetting.Start < scopedSetting.Start)
            //        {
            //            scopedSettingsEntities[scopedSetting.PricingSettingGroup.SettingName] = scopedSetting;
            //        }
            //    }
            //    else
            //    {
            //        scopedSettingsEntities.Add(scopedSetting.PricingSettingGroup.SettingName, scopedSetting);
            //    }
            //}

            //Good solution, goind thru scopes and filter their settings - maybe not effective
            //var scopedSettingsEntitites = await dbContext.PricingSettingTimeScopes.Where(x => x.Start <= from && x.End >= to)
            //    .SelectMany(x=>x.ScopedSettings)
            //    .GroupBy(x=>x.PricingSettingGroup.SettingName)
            //    .Select(x=> x.OrderBy(x=>Math.Abs((x.Start - from).TotalDays)).First())  
            //    .ToDictionaryAsync(x=>x.PricingSettingGroup.SettingName);

            var scopedSettingsEntitites = await dbContext.Settings
                .Where(x => requests.Select(x => x.UniqueSettingName).Contains(x.PricingSettingEntityId))
                .SelectMany(x => x.ScopedSettings)
                .Where(x => x.From <= from && x.To >= to)
                .GroupBy(x => x.Setting.PricingSettingEntityId)
                .Select(x => x.OrderBy(y => Math.Abs((y.From - from).TotalDays)).First())
                .ToDictionaryAsync(x => x.Setting.PricingSettingEntityId);



            IDictionary<string, NullableResult<object>> scopedSettings = new Dictionary<string, NullableResult<object>>();
            foreach (var request in requests)
            {
                if (scopedSettingsEntitites.TryGetValue(request.UniqueSettingName, out PricingScopedSettingEntity foundSetting))
                {
                    scopedSettings.Add(request.UniqueSettingName, new NullableResult<object>(foundSetting.Value, true));
                }
                else
                {
                    scopedSettings.Add(request.UniqueSettingName, new NullableResult<object>(default, false));
                }
            }
            return scopedSettings;
        }

        public void AddTimeScope(string scopeName, DateTime from, DateTime to)
        {
            dbContext.SettingScopes.Add(new PricingSettingScopeEntity() { Name = scopeName, From = from, To = to });
        }
        public void RemoveTimeScope(int scopeId)
        {            
            dbContext.SettingScopes.Remove(new PricingSettingScopeEntity() { PricingSettingScopeEntityId = scopeId});
        }

        public void UpdateTimeScope(TimeScope scope)
        {
            PricingSettingScopeEntity newEntity = new PricingSettingScopeEntity();
            newEntity.PricingSettingScopeEntityId = scope.Id;
            newEntity.From = scope.From;
            newEntity.To = scope.To;            
            var entityEntry = dbContext.Attach(newEntity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<TimeScope> GetTimeScope(int scopeId)
        {
            var scopeEntity = await dbContext.SettingScopes.FindAsync(scopeId);
            var scope = new TimeScope()
            {
                Id = scopeEntity.PricingSettingScopeEntityId
            };
            scope.Name = scopeEntity.Name;
            scope.From = scopeEntity.From;
            scope.To = scope.To;
            return scope;
        }

        public async Task<List<TimeScope>> GetTimeScopes()
        {
            var scopes = (await dbContext.SettingScopes.ToListAsync()).Select(x=>new TimeScope(x.PricingSettingScopeEntityId) {Name = x.Name, From = x.From, To = x.To });
            return scopes.ToList();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }

    }
}
