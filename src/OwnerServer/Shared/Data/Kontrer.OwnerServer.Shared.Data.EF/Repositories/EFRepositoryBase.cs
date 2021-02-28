using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.EF.Repositories
{
    public abstract class EFRepositoryBase<TEntity,TModel> where TEntity : class
    {
        protected readonly DbContext dbContext;

        public EFRepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
            ValidateDbContext(dbContext);
        }

        private static void ValidateDbContext(DbContext dbContext)
        {
            dbContext.Set<TEntity>();
        }

        protected abstract TEntity ToEntity(TModel model);
        protected abstract TModel ToModel(TEntity entity);


    }
}
