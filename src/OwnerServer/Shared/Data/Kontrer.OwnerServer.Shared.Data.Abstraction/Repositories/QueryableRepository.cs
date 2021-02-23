using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
    public abstract class QueryableRepository<TModel, TKey> : CrudRepositoryBase<TModel, TKey>
       where TModel : class
    {

        protected IQueryable<TModel> allRecords;
        protected readonly Func<TModel, TKey> keySelector;
        protected readonly Action<List<RepositoryChange<TModel, TKey>>> saveAction;

        public QueryableRepository(IEnumerable<TModel> allRecords,
            Func<TModel, TKey> keySelector,
            Action<List<RepositoryChange<TModel, TKey>>> saveAction
            ) : this(allRecords.AsQueryable(), keySelector, saveAction)
        {

        }

        public QueryableRepository(IDictionary<TKey, TModel> allRecords,
            Func<TModel, TKey> keySelector,
            Action<List<RepositoryChange<TModel, TKey>>> saveAction
          ) : this(allRecords.Values.AsQueryable(), keySelector, saveAction)
        {

        }

        public QueryableRepository(IQueryable<TModel> allRecords, Func<TModel, TKey> keySelector, Action<List<RepositoryChange<TModel, TKey>>> saveAction)
        {
            this.allRecords = allRecords;
            this.keySelector = keySelector;
            this.saveAction = saveAction;

        }


        public override Task<TModel> TryGetAsync(TKey key)
        {

            var model = allRecords.AsQueryable().FirstOrDefault(x => keySelector(x).Equals(key));
            //return new ValueTask<TModel>(model);
            return Task.FromResult(model);
        }

        public override Task<Dictionary<TKey, TModel>> GetAllAsync()
        {
            Dictionary<TKey, TModel> models = allRecords.AsQueryable().ToDictionary(keySelector);
            return Task.FromResult(models);
        }

      
    }
}
