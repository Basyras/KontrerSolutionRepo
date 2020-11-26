﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Repositories
{
    public interface IGenericRepository<TModel, TKey>  where TModel : class
    {
        Task<Dictionary<TKey, TModel>> GetAllAsync();
        /// <summary>
        /// Returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> TryGetAsync(TKey id);
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
        /// Calling save is required to see results
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        void Update(TKey id, TModel model);
        void Remove(TKey id);
        List<RepositoryChange<TModel, TKey>> Changes { get; }

        //IEnumerable<TModel> Where(Expression<Func<TModel, bool>> selector);
        //IEnumerable<TModel> GetPage(int page, int itemsPerPage, Expression<Func<TModel, bool>> selector = null);
    }
}
