using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories
{

    public interface ICrudRepository<TModel, TKey> : IRepository where TModel : class
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
        void Add(TKey id, TModel model);
        /// <summary>
        /// If id is null it will be genereted
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        void TryAdd(TKey id, TModel model);
        /// <summary>
        /// Updates record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        void Update(TKey id, TModel model);
        /// <summary>
        /// Deletes items with same id.
        /// </summary>
        /// <param name="id"></param>
        void Remove(TKey id);
        
             
     
        
    }
}
