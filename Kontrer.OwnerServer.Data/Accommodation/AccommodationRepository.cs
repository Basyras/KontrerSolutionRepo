using Kontrer.OwnerServer.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.OwnerServer.Data.EntityFramework;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using Kontrer.Shared.Models.Pricing.Costs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Accommodation
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly OwnerServerDbContext dbContext;

        public AccommodationRepository(OwnerServerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Cancel(int id, bool canceledByCustomer, string notes = null)
        {
            throw new NotImplementedException();
        }

        public void Complete(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(int customerId, AccommodationCost cost, AccommodationBlueprint blueprint)
        {
            throw new NotImplementedException();
        }

        public void Edit(AccommodationModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, AccommodationModel>> GetAllAsync()
        {
            var accommodations = dbContext.Accommodations.AsQueryable().ToDictionaryAsync(x=>x.AccommodationId);                
            return accommodations;

        }
        

        public async Task<AccommodationModel> GetAsync(int id)
        {
            var customer = await dbContext.Set<AccommodationModel>().FindAsync(id);
            return customer;

        }

        public Task<PageResult<AccommodationModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern)
        {
            throw new NotImplementedException();
        }
    }
}
