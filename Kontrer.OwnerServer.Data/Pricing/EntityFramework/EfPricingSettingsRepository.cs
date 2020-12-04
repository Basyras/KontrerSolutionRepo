using Kontrer.OwnerServer.Data.Abstraction.Pricing;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing;
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
            entity.TimedSettings= TimedSettingToEntities(model.TimedSettings);
            entity.Type= model.Type;
            return entity;
        }

        internal static TimedSettingModel TimedSettingToModel(TimedSettingEntity entity)
        {

            TimedSettingModel model = new TimedSettingModel(entity.PricingSettingGroupId,entity.PricingSettingGroup.SettingName,entity.PricingSettingGroup.Type,entity.Start,entity.End,entity.Value);
            return model;
        }

        internal static List<TimedSettingModel> TimedSettingToModels(List<TimedSettingEntity> entities)
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

        internal static TimedSettingEntity TimedSettingToEntity(TimedSettingModel model)
        {
            TimedSettingEntity entity = new TimedSettingEntity();
            entity.End = model.End;            
            entity.PricingSettingGroupId= model.SettingGroupId;
            entity.Start = model.Start;
            entity.Value= model.Value;

            return entity;
        }

        internal static List<TimedSettingEntity> TimedSettingToEntities(List<TimedSettingModel> models)
        {
            List<TimedSettingEntity> entities = new List<TimedSettingEntity>(models.Count);
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
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public void EditTimedSetting(TimedSettingSelector selector, TimedSettingModel timedSetting)
        {
            throw new NotImplementedException();
        }

        public Task<NullableResult<TSetting>> GetTimedSetting<TSetting>(TimedSettingSelector selector)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, NullableResult<object>> GetTimedSettings(List<TimedSettingSelector> selectors)
        {
            throw new NotImplementedException();
        }

        public void RemoveTimedSetting(TimedSettingSelector selector)
        {
            throw new NotImplementedException();
        }

    }
}
