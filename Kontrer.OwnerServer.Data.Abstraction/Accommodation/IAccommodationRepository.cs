using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Accommodation
{
    /// <summary>
    /// Changes must be commited with unit of work in order to be persistent
    /// </summary>
    public interface IAccommodationRepository : IRepository
    {
        Task<Dictionary<int, AccommodationModel>> GetAllAsync();
        Task<AccommodationModel> GetAsync(int id);
        Task<PageResult<AccommodationModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern);
        void Create(int customerId, AccommodationCost cost, AccommodationBlueprint blueprint);
        /// <summary>
        /// Customer payed the deposit
        /// </summary>
        /// <param name="id"></param>
        void Complete(int id);
        /// <summary>
        /// Customer canceled the order of accommodation
        /// </summary>
        /// <param name="id"></param>
        void Cancel(int id, bool canceledByCustomer, string notes = null);

        void Edit(AccommodationModel model);
    }
}
