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
    public abstract class EFInstantCrudRepositoryBase<TEntity, TEntityKey, TModel, TModelKey> : EFRepositoryBase<TEntity, TModel>, IInstantCrudRepository<TModel, TModelKey>
        where TModel : class
        where TEntity : class, new()
        where TEntityKey : TModelKey
    {
        private readonly Func<TEntity, TEntityKey> entityIdSelector;

        public EFInstantCrudRepositoryBase(DbContext dbContext, Expression<Func<TEntity, TEntityKey>> entityIdPropertyNameSelector) : base(dbContext)
        {
            this.entityIdSelector = entityIdPropertyNameSelector.Compile();
        }

        protected abstract TModelKey GetModelId(TModel model);

        protected abstract void SetEntityId(TModelKey id, TEntity entity);

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

        public async Task<TModel> AddAsync(TModel model)
        {
            dbContext.Add(ToEntity(model));
            await dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<Dictionary<TModelKey, TModel>> GetAllAsync()
        {
            var models = await dbContext.Set<TEntity>().AsQueryable().ToDictionaryAsync(x => (TModelKey)GetEntityId(x), x => ToModel(x));
            return models;
        }

        public void Remove(TModelKey id)
        {
            TEntity entity = new TEntity();
            SetEntityId(id, entity);
            dbContext.Remove(entity);
            dbContext.SaveChanges();
        }

        public async Task<TModel> GetAsync(TModelKey id)
        {
            var entity = await dbContext.Set<TEntity>().FindAsync(id);
            if (entity == null)
                throw new InvalidOperationException($"Can't find entity with id: '{id}'");
            return ToModel(entity);
        }

        public async Task<TModel> TryGetAsync(TModelKey id)
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

            dbContext.SaveChanges();
            var updatedModel = ToModel(updatetedEntity);
            return updatedModel;
        }

        public Task<TModel> UpdateAsync(TModel model)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(TModelKey id)
        {
            throw new NotImplementedException();
        }
    }
}