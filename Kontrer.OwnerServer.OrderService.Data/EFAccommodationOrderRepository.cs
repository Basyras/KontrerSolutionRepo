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
        public void AddOrder(AccommodationOrder order)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public void EditOrder(AccommodationOrder order)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, AccommodationOrder>> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public void RemoveOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
