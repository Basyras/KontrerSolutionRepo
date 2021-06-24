using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Repositories.EF
{
    public abstract class EFRepositoryBase<TEntity, TModel> where TEntity : class
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