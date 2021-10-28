using Kontrer.OwnerServer.CustomerService.Application.Interfaces;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.Shared.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Infrastructure.EntityFramework
{
    public class EFCustomerRepository : EFInstantCrudRepositoryBase<CustomerEntity, int>, ICustomerRepository
    {
        public EFCustomerRepository(DbContext dbContext) : base(dbContext, x => x.Id)
        {
        }

        public async Task<List<CustomerEntity>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var customers = await dbContext.Set<CustomerEntity>().AsQueryable().Where(customer => ids.Any(x => x == customer.Id)).ToListAsync();
            return customers;
        }
    }
}