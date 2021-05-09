using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.EF.Repositories
{
    public abstract class EFCrudRepositoryBase<TEntity, TModel, TKey, TEntityKey> : EFRepositoryBase<TEntity, TModel>, IInstantCrudRepository<TModel, TKey>
        where TModel : class
        where TEntity : class, new()
        where TEntityKey : TKey
    {

        private readonly Func<TEntity, TEntityKey> entityIdSelector;

        public EFCrudRepositoryBase(DbContext dbContext, Expression<Func<TEntity, TEntityKey>> entityIdPropertyNameSelector) : base(dbContext)
        {

            this.entityIdSelector = entityIdPropertyNameSelector.Compile();

        }


        protected abstract TKey GetModelId(TModel model);
        protected abstract void SetEntityId(TKey id, TEntity entity);

        protected TEntityKey GetEntityId(TEntity entity)
        {
            return entityIdSelector(entity);
        }


        private static Expression<Func<TEntity, bool>> GetEntityIdSelectorExpression(int personId, Expression<Func<TEntity, TEntityKey>> idSelector)
        {
            var propertySelector = (MemberExpression)idSelector.Body;
            string name = propertySelector.Member.Name;
            ConstantExpression constant = Expression.Constant(personId, typeof(TEntityKey));
            ParameterExpression personParam = Expression.Parameter(typeof(TEntity));
            MemberExpression property = Expression.Property(personParam, name);
            BinaryExpression equals = Expression.Equal(property, constant);
            Expression<Func<TEntity, bool>> finalSelector = Expression.Lambda<Func<TEntity, bool>>(equals, new[] { personParam });
            return finalSelector;
        }

        public TModel AddAsync(TModel model)
        {
            dbContext.Add(ToEntity(model));
            return model;
        }

        public async Task<Dictionary<TKey, TModel>> GetAllAsync()
        {
            var models = await dbContext.Set<TEntity>().AsQueryable().ToDictionaryAsync(x => (TKey)GetEntityId(x), x => ToModel(x));
            return models;
        }

        public void Remove(TKey id)
        {
            TEntity entity = new TEntity();
            SetEntityId(id, entity);
            dbContext.Remove(entity);
        }

        public async Task<TModel> TryGetAsync(TKey id)
        {
            var entity = await dbContext.Set<TEntity>().FindAsync(id);
            return ToModel(entity);
        }


        public TModel Update(TModel model)
        {
            var entityToUpdate = ToEntity(model);
            var modelId = GetModelId(model);
            TEntity updatetedEntity;

            var oldEntityEntry = dbContext.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => GetEntityId(x.Entity).Equals(modelId));
            if (oldEntityEntry is not null)
            {
                oldEntityEntry.CurrentValues.SetValues(entityToUpdate);
                updatetedEntity = oldEntityEntry.Entity;
            }
            else
            {
                updatetedEntity = dbContext.Update(entityToUpdate).Entity;
            }

            var updatedModel = ToModel(updatetedEntity);
            return updatedModel;

        }



    }
}
