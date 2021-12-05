using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.Repositories
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
        TModel Add(TModel model);

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        void Update(TModel model);

        /// <summary>
        /// Deletes item with same id.
        /// </summary>
        /// <param name="id"></param>
        void Remove(TKey id);
    }
}