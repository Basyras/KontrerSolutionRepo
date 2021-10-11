using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Repositories.EF
{
    public abstract class EFRepositoryBase<TEntity, TModel> where TEntity : class
    {
        private static bool isInitialized = false;
        protected readonly DbContext dbContext;

        public EFRepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
            if (isInitialized is false)
            {
                ValidateDbContext(dbContext);
            }
            isInitialized = true;
        }

        /// <summary>
        /// Checks if generic DbContext contains a requiered Set<<see cref="TEntity"/>>
        /// </summary>
        /// <param name="dbContext"></param>
        private static void ValidateDbContext(DbContext dbContext)
        {
            dbContext.Set<TEntity>();
        }

        protected abstract TEntity ToEntity(TModel model);

        protected abstract TModel ToModel(TEntity entity);
    }
}