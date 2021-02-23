using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.EF.Repositories
{
    public class EFCrudRepository<TEntity, TModel, TKey> : CrudRepositoryBase<TModel, TKey> where TModel : class
    {
        public EFCrudRepository()
        {

        }

        public override Task<Dictionary<TKey, TModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override PageResult<TModel> GetPage(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public override Task<TModel> TryGetAsync(TKey key)
        {
            throw new NotImplementedException();
        }


    }
}
