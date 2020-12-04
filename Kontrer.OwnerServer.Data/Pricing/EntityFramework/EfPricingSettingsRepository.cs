using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Pricing.EntityFramework
{

    public class EfPricingSettingsRepository : IPricingSettingsRepository
    {
        private readonly OwnerServerDbContext dbContext;

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

        internal static TimedSettingModel TimedSettingToModel(PricingTimedSettingEntity entity)
        {
            TimedSettingModel model = new TimedSettingModel(entity.PricingSettingGroupId, entity.PricingSettingGroup.SettingName, entity.PricingSettingGroup.Type, entity.Start, entity.End, entity.Value);
            return model;
        }

        internal static List<TimedSettingModel> TimedSettingToModels(List<PricingTimedSettingEntity> entities)
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

        internal static PricingTimedSettingEntity TimedSettingToEntity(TimedSettingModel model)
        {
            PricingTimedSettingEntity entity = new PricingTimedSettingEntity();
            entity.End = model.End;
            entity.PricingSettingGroupId = model.SettingGroupId;
            entity.Start = model.Start;
            entity.Value = model.Value;

            return entity;
        }

        internal static List<PricingTimedSettingEntity> TimedSettingToEntities(List<TimedSettingModel> models)
        {
            List<PricingTimedSettingEntity> entities = new List<PricingTimedSettingEntity>(models.Count);
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


        public EfPricingSettingsRepository(OwnerServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddTimedSetting(TimedSettingModel timedSetting)
        {
            dbContext.PricingTimedSettings.Add(TimedSettingToEntity(timedSetting));
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public void EditTimedSetting(TimedSettingModel timedSetting)
        {
            PricingTimedSettingEntity entity = TimedSettingToEntity(timedSetting);
            var entityEntry = dbContext.Attach(entity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<NullableResult<TSetting>> GetTimedSetting<TSetting>(TimedSettingSelector selector)
        {
            var setting = await dbContext.PricingTimedSettings.FirstOrDefaultAsync(x => x.PricingSettingGroup.SettingName == selector.SettingUniqueName && x.Start == selector.Start && x.End == selector.End);
            if (setting != null)
            {
                return new NullableResult<TSetting>((TSetting)setting.Value, true, default);
            }
            else
            {
                return new NullableResult<TSetting>(default, false, default);
            }
        }
     

        public async Task<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, NullableResult<object>>>> GetTimedSettingsAsync(List<TimedSettingSelector> selectors)
        {
            var query = dbContext.PricingTimedSettings.Where(x => selectors.Any(y => y.SettingUniqueName == x.PricingSettingGroup.SettingName && y.Start == x.Start && y.End == x.End));
            var dic = await query.ToDictionaryAsync(x => x.PricingSettingGroup.SettingName, x => (IDictionary<Tuple<DateTime,DateTime>, NullableResult<object>>)x.PricingSettingGroup.TimedSettings.ToDictionary(y=>new Tuple<DateTime,DateTime>(y.Start,y.End),y=> 
            {                
                return y == null ? new NullableResult<object>(default, false) : new NullableResult<object>(y.Value, true);                     
            }));

            return dic;
        }

        public void RemoveTimedSetting(int settingGroupId,DateTime start, DateTime end)
        {
            PricingTimedSettingEntity entity = new PricingTimedSettingEntity()
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
