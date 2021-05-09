using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{
    public interface IBulkCrudRepository<TModel, TKey> : IBulkRepository
    {
        /// <summary>
        /// Returns all records as dictionary
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<TKey, TModel>> GetAllAsync();
        /// <summary>
        /// Returns default when not found, 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#nullable enable
        Task<TModel?> TryGetAsync(TKey id);
#nullable disable
        /// <summary>
        /// If id is null it will be genereted
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        Task<TModel> AddAsync(TModel model);
        /// <summary>
        /// Updates record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        void UpdateAsync(TModel model);
        /// <summary>
        /// Deletes items with same id.
        /// </summary>
        /// <param name="id"></param>
        void RemoveAsync(TKey id);
    }
}
