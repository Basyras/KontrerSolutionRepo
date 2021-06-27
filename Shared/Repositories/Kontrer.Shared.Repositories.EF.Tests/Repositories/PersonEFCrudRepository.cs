using Kontrer.OwnerServer.Shared.Data.EF.Tests;
using Kontrer.Shared.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.Repositories.EF.Tests.Repositories
{
    public class PersonEFCrudRepository : EFInstantCrudRepositoryBase<PersonEntity, int, PersonModel, int>
    {
        public PersonEFCrudRepository(DbContext dbContext) : base(dbContext, x => x.Id)
        {
        }

        protected override int GetModelId(PersonModel model)
        {
            return model.Id;
        }

        protected override void SetEntityId(int id, ref PersonEntity entity)
        {
            entity.Id = id;
        }

        protected override void SetModelId(int id, ref PersonModel model)
        {
            model.Id = id;
        }

        protected override PersonEntity ToEntity(PersonModel model)
        {
            if (model == null)
            {
                return null;
            }

            var entity = new PersonEntity();
            entity.Id = model.Id;
            entity.Age = model.Age;
            entity.Name = model.Name;

            return entity;
        }

        protected override PersonModel ToModel(PersonEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var model = new PersonModel();
            model.Id = entity.Id;
            model.Age = entity.Age;
            model.Name = entity.Name;

            return model;
        }
    }
}