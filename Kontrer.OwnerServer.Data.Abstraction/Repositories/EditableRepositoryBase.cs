using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Repositories
{
    public abstract class EditableRepositoryBase<TModel, TKey> : IGenericRepository<TModel, TKey>
           where TModel : class
    {
        public List<RepositoryChange<TModel, TKey>> Changes { get; private set; } = new List<RepositoryChange<TModel, TKey>>();

        public abstract Task<TModel> TryGetAsync(TKey key);

        public abstract Task<Dictionary<TKey, TModel>> GetAllAsync();

        public void Remove(TKey id)
        {
            var oldUpdate = Changes.FirstOrDefault(x => x.Id.Equals(id));
            if (oldUpdate == null)
            {
                Changes.Add(new RepositoryChange<TModel, TKey>(id,null,PriceChangedActions.Removed));
            }
            else
            {
                switch (oldUpdate.Action)
                {
                    case PriceChangedActions.Added:
                        Changes.Remove(oldUpdate);
                        break;
                    case PriceChangedActions.Modified:
                        Changes.Remove(oldUpdate);
                        break;
                    case PriceChangedActions.Removed:
                        break;
                }
            }
        }

        public void TryAdd(TKey key, TModel model)
        {
            var oldExists = TryGetAsync(key) != null;
            if (oldExists == false)
            {
                Changes.Add(new RepositoryChange<TModel, TKey>(key, model, PriceChangedActions.Added));
            }
        }

        public void Add(TKey id, TModel model)
        {
            var newUpdate = new RepositoryChange<TModel, TKey>(id, model, PriceChangedActions.Added);
            Changes.Add(newUpdate);
        }

        public void Update(TKey id, TModel model)
        {
            var newUpdate = new RepositoryChange<TModel, TKey>(id, model, PriceChangedActions.Modified);
            var oldUpdate = Changes.FirstOrDefault(x => x.Id.Equals(id));
            if (oldUpdate == null)
            {
                Changes.Add(newUpdate);
            }
            else
            {
                var index = Changes.IndexOf(oldUpdate);
                newUpdate.Action = oldUpdate.Action;
                Changes[index] = newUpdate;
            }
        }



    }
}
