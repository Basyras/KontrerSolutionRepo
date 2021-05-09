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

        public List<RepositoryAction<TimedSettingModel, int>> Actions => throw new NotImplementedException();

        internal static PricingSettingGroupModel GroupToModel(PricingSettingGroupEntity entity)
        {
            PricingSettingGroupModel model = new PricingSettingGroupModel(entity.Type, entity.PricingSettingGroupId, entity.SettingName, TimedSettingToModels(entity.TimedSettings));
            return model;
        }

        internal static PricingSettingGroupEntity GroupToEntity(PricingSettingGroupModel model)
        {
            var entity = new PricingSettingGroupEntity();
            entity.PricingSettingGroupId = model.SettingId;
            entity.SettingName = model.SettingName;
            entity.TimedSettings = TimedSettingToEntities(model.TimedSettings);
            entity.Type = model.Type;
            return entity;
        }

        internal static TimedSettingModel TimedSettingToModel(PricingScopedSettingEntity entity)
        {
            TimedSettingModel model = new TimedSettingModel(entity.PricingSettingGroupId, entity.PricingSettingGroup.SettingName, entity.PricingSettingGroup.Type, entity.Start, entity.End, entity.Value);
            return model;
        }

        internal static List<TimedSettingModel> TimedSettingToModels(List<PricingScopedSettingEntity> entities)
        {
            List<TimedSettingModel> models = new List<TimedSettingModel>(entities.Count);
            foreach (var entity in entities)
            {
                var model = TimedSettingToModel(entity);
                if (model != null)
                {
                    models.Add(model);
                }
            }

            return models;
        }

        internal static PricingScopedSettingEntity TimedSettingToEntity(TimedSettingModel model)
        {
            PricingScopedSettingEntity entity = new PricingScopedSettingEntity();
            entity.End = model.End;
            entity.PricingSettingGroupId = model.SettingGroupId;
            entity.Start = model.Start;
            entity.Value = model.Value;

            return entity;
        }

        internal static List<PricingScopedSettingEntity> TimedSettingToEntities(List<TimedSettingModel> models)
        {
            List<PricingScopedSettingEntity> entities = new List<PricingScopedSettingEntity>(models.Count);
            foreach (var model in models)
            {
                var entity = TimedSettingToEntity(model);
                if (entity != null)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }


        public EfPricingSettingsRepository(PricingServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddTimedSetting(TimedSettingModel timedSetting)
        {
            dbContext.PricingScopedSettings.Add(TimedSettingToEntity(timedSetting));
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public void EditTimedSetting(TimedSettingModel timedSetting)
        {
            PricingScopedSettingEntity entity = TimedSettingToEntity(timedSetting);
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<NullableResult<TSetting>> GetScopedSettingAsync<TSetting>(DateTime from, DateTime to, SettingRequest<TSetting> request)
        {            
            var settingEntity = await dbContext.PricingSettingGroups.AsQueryable()
                .Where(x => x.SettingName == request.UniqueSettingName)
                .SelectMany(x => x.TimedSettings)
                .Where(x => x.Start <= from && x.End >= to)
                .OrderBy(x=>Math.Abs((x.Start - from).TotalDays))
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

            var scopedSettingsEntitites = await dbContext.PricingSettingGroups
                .Where(x => requests.Select(x => x.UniqueSettingName).Contains(x.SettingName))
                .SelectMany(x => x.TimedSettings)
                .Where(x => x.Start <= from && x.End >= to)
                .GroupBy(x => x.PricingSettingGroup.SettingName)
                .Select(x => x.OrderBy(y => Math.Abs((y.Start - from).TotalDays)).First())
                .ToDictionaryAsync(x => x.PricingSettingGroup.SettingName);



            IDictionary<string, NullableResult<object>> scopedSettings = new Dictionary<string, NullableResult<object>>();
            foreach (var request in requests)
            {
                if(scopedSettingsEntitites.TryGetValue(request.UniqueSettingName,out PricingScopedSettingEntity foundSetting))
                {
                    scopedSettings.Add(request.UniqueSettingName, new NullableResult<object>(foundSetting.Value,true));
                }
                else
                {
                    scopedSettings.Add(request.UniqueSettingName, new NullableResult<object>(default, false));
                }
            }
            return scopedSettings;
        }

        public void RemoveTimedSetting(int settingGroupId, DateTime start, DateTime end)
        {
            PricingScopedSettingEntity entity = new PricingScopedSettingEntity()
            {
                PricingSettingGroupId = settingGroupId,
                Start = start,
                End = end
            };
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Deleted;
        }

    }
}
