﻿using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.Shared.Data.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.EF.Tests.Repositories
{
    public class CarEFCrudRepository : EFInstantCrudRepositoryBase<CarEntity, CarModel, int, int>
    {
        public CarEFCrudRepository(TestDbContext dbContext) : base(dbContext,x=>x.Id)
        {

        }
      

        protected override int GetModelId(CarModel model)
        {
            return model.Id;
        }

        protected override void SetEntityId(int id, CarEntity entity)
        {
            entity.Id = id;
        }

        protected override CarEntity ToEntity(CarModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = new CarEntity();
            entity.Age = model.Age;
            entity.Id = model.Id;
            entity.Name = model.Name;
            entity.CustomerId = model.CustomerId;
            return entity;

        }

        protected override CarModel ToModel(CarEntity entity)
        {
            if(entity == null)
            {
                return null;
            }

            var model = new CarModel();
            model.Age = entity.Age;
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.CustomerId = entity.CustomerId;
            return model;
        }

        
    }
}
