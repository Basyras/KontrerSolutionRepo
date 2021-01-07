using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation
{
    /// <summary>
    /// Changes must be commited with unit of work in order to be persistent
    /// </summary>
    public interface IAccommodationRepository : IRepository
    {
        Task<Dictionary<int, FinishedAccommodationModel>> GetAllAsync();
        Task<FinishedAccommodationModel> GetAsync(int id);
        Task<PageResult<FinishedAccommodationModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern);

        void Add(FinishedAccommodationModel model);      
        /// <summary>
        /// Customer canceled the order of accommodation
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);

        void Edit(FinishedAccommodationModel model);
    }
}
