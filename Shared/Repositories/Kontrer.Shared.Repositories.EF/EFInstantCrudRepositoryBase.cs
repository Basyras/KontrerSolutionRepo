using Kontrer.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Repositories.EF
{
    public abstract class EFInstantCrudRepositoryBase<TEntity, TEntityKey, TModel, TModelKey> : EFRepositoryBase<TEntity, TModel>, IInstantCrudRepository<TModel, TModelKey>
        where TModel : class
        where TEntity : class, new()
    {
        private readonly Func<TEntity, TEntityKey> entityIdSelector;

        public EFInstantCrudRepositoryBase(DbContext dbContext, Expression<Func<TEntity, TEntityKey>> entityIdPropertyNameSelector) : base(dbContext)
        {
            entityIdSelector = entityIdPropertyNameSelector.Compile();
        }

        protected abstract TModelKey GetModelId(TModel model);

        protected abstract void SetModelId(TEntityKey id, ref TModel model);

        protected abstract void SetEntityId(TModelKey id, ref TEntity entity);

        protected abstract TModelKey ToModelId(TEntityKey id);

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
            var entity = ToEntity(model);
            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();
            SetModelId(GetEntityId(entity), ref model);
            return model;
        }

        public async Task<Dictionary<TModelKey, TModel>> GetAllAsync()
        {
            var models = await dbContext.Set<TEntity>().AsQueryable().ToDictionaryAsync(x => ToModelId(GetEntityId(x)), x => ToModel(x));
            return models;
        }

        public void Remove(TModelKey id)
        {
            TEntity entity = new TEntity();
            SetEntityId(id, ref entity);
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

        public async Task RemoveAsync(TModelKey id)
        {
            //dbContext.Set<TEntity>().Remove(entityToUpdate);
            var oldEntry = dbContext.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => ToModelId(GetEntityId(x.Entity)).Equals(id));
            if (oldEntry != null)
            {
                dbContext.Set<TEntity>().Remove(oldEntry.Entity);
            }
            else
            {
                var entityToUpdate = new TEntity();
                SetEntityId(id, ref entityToUpdate);
                dbContext.Set<TEntity>().Remove(entityToUpdate);
            }

            await dbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Use when model class should be used as ef entity class
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TModelKey"></typeparam>
    public abstract class EFInstantCrudRepositoryBase<TModel, TModelKey> : EFInstantCrudRepositoryBase<TModel, TModelKey, TModel, TModelKey>
       where TModel : class, new()
    {
        protected EFInstantCrudRepositoryBase(DbContext dbContext, Expression<Func<TModel, TModelKey>> entityIdPropertyNameSelector) : base(dbContext, entityIdPropertyNameSelector)
        {
        }

        protected override void SetEntityId(TModelKey modelId, ref TModel entity)
        {
            SetModelId(modelId, ref entity);
        }

        protected override TModel ToEntity(TModel model)
        {
            return model;
        }

        protected override TModel ToModel(TModel entity)
        {
            return entity;
        }

        protected override TModelKey ToModelId(TModelKey key)
        {
            return key;
        }
    }
}