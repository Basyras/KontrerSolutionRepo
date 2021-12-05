using Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework;
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
using Basyc.Shared.Models;

namespace Kontrer.OwnerServer.PricingService.Infrastructure.EntityFramework
{

    public class EfPricingSettingsRepository : ISettingsRepository
    {
        private readonly PricingServiceDbContext dbContext;




        public EfPricingSettingsRepository(PricingServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddScopedSetting<TSetting>(string settingName, int timeScopeId, TSetting value)
        {
            dbContext.ScopedSettings.Add(new PricingScopedSettingEntity()
            {
                SettingId = settingName,
                TimeScopeId = timeScopeId,
                Value = Serialize(value),
            });
        }

        public void AddScopedSetting(string settingName, int timeScopeId, object value, Type settingType)
        {
            dbContext.ScopedSettings.Add(new PricingScopedSettingEntity()
            {
                SettingId = settingName,
                TimeScopeId = timeScopeId,
                Value = Serialize(value, settingType),
            });
        }


        public void RemoveScopedSetting(string settingName, int timeScopeId)
        {
            PricingScopedSettingEntity entity = new PricingScopedSettingEntity()
            {
                Setting = new PricingSettingEntity() { PricingSettingEntityId = settingName },
                TimeScope = new PricingSettingTimeScopeEntity() { PricingSettingScopeEntityId = timeScopeId },
            };
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public void UpdateScopedSetting(string settingName, int timeScopeId, object value,Type settingType)
        {
            PricingScopedSettingEntity newEntity = new PricingScopedSettingEntity();
            newEntity.Setting = new PricingSettingEntity { PricingSettingEntityId = settingName };
            newEntity.TimeScope = new PricingSettingTimeScopeEntity() { PricingSettingScopeEntityId = timeScopeId };
            newEntity.Value = Serialize(value, settingType);

            var entityEntry = dbContext.Attach(newEntity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<NullableResult<TSetting>> GetScopedSettingAsync<TSetting>(ScopedSettingRequest<TSetting> request)
        {


            var settingEntity = await dbContext.Settings.AsQueryable()
                .Where(x => x.PricingSettingEntityId == request.SettingRequest.UniqueSettingName)
                .SelectMany(x => x.ScopedSettings)
                .FirstAsync(x => x.TimeScope.PricingSettingScopeEntityId == request.TimeScopeId);
                

            if (settingEntity != null)
            {
                return new NullableResult<TSetting>((TSetting)Deserialize(settingEntity.Value,settingEntity.Setting.Type), true);
            }
            else
            {
                return new NullableResult<TSetting>(default, false);
            }
        }

        public async Task<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>> GetAllScopedSettingsAsync()
        {
            //var settingEntities = await dbContext.Settings
            //    .Select(settingEntity =>
            //    new 
            //    {
            //        Name = settingEntity.PricingSettingEntityId, 
            //        Settings = settingEntity.ScopedSettings.ToDictionary(scopedEntity => new Tuple<DateTime, DateTime>(scopedEntity.TimeScope.From, scopedEntity.TimeScope.To), scopedEntity => new NullableResult<object>(scopedEntity.Value, true, default)) 
            //    })
            //    .ToDictionaryAsync(dicPair => dicPair.Name, dicPair => dicPair.Settings);

            var settingEntities = await dbContext.Settings               
                .Include(x=>x.ScopedSettings)
                .ThenInclude(x=>x.TimeScope)
                .Select(settingEntity =>
                new
                {
                    Name = settingEntity.PricingSettingEntityId,
                    Settings = settingEntity.ScopedSettings
                })              
                .ToListAsync();

            var settings = settingEntities
                .ToDictionary(
                entity => entity.Name,
                settingEntity => settingEntity.Settings
                   .ToDictionary(scopedEntity => new Tuple<DateTime, DateTime>(scopedEntity.TimeScope.From, scopedEntity.TimeScope.To), y => new NullableResult<object>(y.Value, true)) as IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>
                ) as IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>;

            return settings;
        }


        public async Task<IDictionary<string, NullableResult<object>>> GetScopedSettingsAsync(IEnumerable<ScopedSettingRequest> requests)
        {             
            List<string> settingsKeys = requests.Select(x => x.SettingRequest.UniqueSettingName).ToList();
            List<int> scopedKeys = requests.Select(x => x.TimeScopeId).ToList();

            //Dictionary<string, PricingScopedSettingEntity> scopedSettingsEntitites = await dbContext.Settings
            //    .Include(x => x.ScopedSettings)
            //    .ThenInclude(x => x.TimeScope)
            //    .Where(x => settingsKeys.Contains(x.PricingSettingEntityId))
            //    .SelectMany(x => x.ScopedSettings.Where(x => scopedKeys.Contains(x.TimeScope.PricingSettingScopeEntityId)))
            //    .ToDictionaryAsync(x => x.Setting.PricingSettingEntityId);


            //TODO TEST WITH UNIT TEST SQL QUERY WITH DEBUG VIEW PROPERTY
            //var scopedSettingsEntitites = dbContext.Settings
            //     .Include(x => x.ScopedSettings)
            //     .ThenInclude(x => x.TimeScope)
            //     .Where(x => settingsKeys.Contains(x.PricingSettingEntityId))
            //     .Join(requests, x => x.PricingSettingEntityId, x => x.SettingRequest.UniqueSettingName, (x, y) => new { Setting = x, TimeScopeId = y.TimeScopeId })
            //     .Select(x => x.Setting.ScopedSettings.First(y => y.TimeScope.PricingSettingScopeEntityId == x.TimeScopeId));

            //var scopedSettingsEntitites = dbContext.Settings.Where(x => x.PricingSettingEntityId == "asd");

            var settingEntities = await dbContext.Settings
                 .Include(x => x.ScopedSettings)
                 .ThenInclude(x => x.TimeScope)
                 .Where(x => settingsKeys.Contains(x.PricingSettingEntityId)).ToListAsync();

            var settings = settingEntities
                .Join(requests, x => x.PricingSettingEntityId, x => x.SettingRequest.UniqueSettingName, (x, y) => new { Setting = x, TimeScopeId = y.TimeScopeId })
                .Select(x =>
                {                   
                    var entity = x.Setting.ScopedSettings.FirstOrDefault(y => y.TimeScope.PricingSettingScopeEntityId == x.TimeScopeId);
                    NullableResult<object> nullableResult;
                    if (entity != null)
                    {
                        nullableResult =  new NullableResult<object>(Deserialize(entity.Value,entity.Setting.Type), true);
                    }
                    else
                    {
                        nullableResult = new NullableResult<object>(null,false);
                    }
                    return new { SettingName = x.Setting.PricingSettingEntityId, Result = nullableResult };
                }).ToDictionary(x=>x.SettingName,x=>x.Result) as IDictionary<string, NullableResult<object>>;

            return settings;

            //var sql = scopedSettingsEntitites.ToQueryString();

            //throw new NotImplementedException();
            //IDictionary<string, NullableResult<object>> scopedSettings = new Dictionary<string, NullableResult<object>>();
            //foreach (var request in requests)
            //{
            //    if (scopedSettingsEntitites.TryGetValue(request.SettingRequest.UniqueSettingName, out PricingScopedSettingEntity foundSetting))
            //    {
            //        scopedSettings.Add(request.SettingRequest.UniqueSettingName, new NullableResult<object>(foundSetting.Value, true));
            //    }
            //    else
            //    {
            //        scopedSettings.Add(request.SettingRequest.UniqueSettingName, new NullableResult<object>(default, false));
            //    }
            //}
            //return scopedSettings;
        }

        public void CreateNewTimeScope(string scopeName, DateTime from, DateTime to)
        {
            dbContext.SettingTimeScopes.Add(new PricingSettingTimeScopeEntity() { Name = scopeName, From = from, To = to });
        }
        public void RemoveTimeScope(int scopeId)
        {
            dbContext.SettingTimeScopes.Remove(new PricingSettingTimeScopeEntity() { PricingSettingScopeEntityId = scopeId });
        }

        public void UpdateTimeScope(TimeScope scope)
        {
            PricingSettingTimeScopeEntity newEntity = new PricingSettingTimeScopeEntity();
            newEntity.PricingSettingScopeEntityId = scope.Id;
            newEntity.From = scope.From;
            newEntity.To = scope.To;
            var entityEntry = dbContext.Attach(newEntity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<TimeScope> GetTimeScope(int scopeId)
        {
            var scopeEntity = await dbContext.SettingTimeScopes.FindAsync(scopeId);
            var scope = new TimeScope()
            {
                Id = scopeEntity.PricingSettingScopeEntityId
            };
            scope.Name = scopeEntity.Name;
            scope.From = scopeEntity.From;
            scope.To = scope.To;
            return scope;
        }

        public async Task<List<TimeScope>> GetTimeScopesAsync()
        {
            var scopes = (await dbContext.SettingTimeScopes.ToListAsync()).Select(x => new TimeScope(x.PricingSettingScopeEntityId) { Name = x.Name, From = x.From, To = x.To });
            return scopes.ToList();
        }




        public async Task<Dictionary<string, Type>> GetSettingsAsync()
        {
            var settings = await dbContext.Settings.ToDictionaryAsync(x => x.PricingSettingEntityId, x => x.Type);
            return settings;
        }
        public void CreateNewSetting<TSetting>(string settindId)
        {
            CreateNewSetting(settindId, typeof(TSetting));
        }
        public void CreateNewSetting(string settindId, Type settingType)
        {            
            dbContext.Settings.Add(new PricingSettingEntity() { PricingSettingEntityId = settindId, Type = settingType });
        }


        public void RemoveSetting(string settindId)
        {
            dbContext.Settings.Remove(new PricingSettingEntity() { PricingSettingEntityId = settindId });
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

        private static string Serialize<TSetting>(TSetting value)
        {
            return System.Text.Json.JsonSerializer.Serialize(value);
        }

        private static string Serialize(object value,Type setingType)
        {
            return System.Text.Json.JsonSerializer.Serialize(value, setingType);
        }

        private static TValue Deserialize<TValue>(string value)
        {
            return System.Text.Json.JsonSerializer.Deserialize<TValue>(value);
        }

        private static object Deserialize(string value, Type type)
        {
            return System.Text.Json.JsonSerializer.Deserialize(value, type);
        }
    }
}
