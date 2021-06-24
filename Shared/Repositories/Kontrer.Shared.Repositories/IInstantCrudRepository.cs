﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Repositories
{
    public interface IInstantCrudRepository<TModel, TKey> : IInstantRepository
    {
        /// <summary>
        /// Returns all records as dictionary
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<TKey, TModel>> GetAllAsync();

        /// <summary>
        /// Throws exception when not found,
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetAsync(TKey id);

        /// <summary>
        /// Returns default when not found,
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
#nullable enable

        Task<TModel?> TryGetAsync(TKey id);

#nullable disable

        /// <summary>
        /// If id is null it will be genereted, After executing properties like Ids are updated
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        Task<TModel> AddAsync(TModel model);

        /// <summary>
        /// Updates record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        Task<TModel> UpdateAsync(TModel model);

        /// <summary>
        /// Deletes items with same id.
        /// </summary>
        /// <param name="id"></param>
        Task RemoveAsync(TKey id);
    }
}