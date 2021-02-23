using Kontrer.OwnerServer.OrderService.Data.Abstraction;
using Kontrer.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kontrer.OwnerServer.OrderService.Data
{
    public class EFAccommodationOrderRepository : IAccommodaionOrderRepository
    {
        private readonly DbContext dbContext;

        public EFAccommodationOrderRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(int id, AccommodationOrder model)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public Task<Dictionary<int, AccommodationOrder>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void TryAdd(int id, AccommodationOrder model)
        {
            throw new NotImplementedException();
        }

        public Task<AccommodationOrder> TryGetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, AccommodationOrder model)
        {
            throw new NotImplementedException();
        }
    }
}
