﻿using Basyc.Repositories.EF;
using Kontrer.OwnerServer.CustomerService.Application.Customer;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Infrastructure.EntityFramework
{
	public class EFCustomerRepository : EFInstantCrudRepositoryBase<CustomerEntity, int>, ICustomerRepository
    {
        public EFCustomerRepository(CustomerServiceDbContext dbContext, ILogger<EFCustomerRepository> logger) : base(dbContext, x => x.Id,logger)
        {
        }

        public async Task<List<CustomerEntity>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var customers = await dbContext.Set<CustomerEntity>()
                .AsQueryable()
                .Where(customer => ids.Any(x => x == customer.Id))
                .ToListAsync();

            return customers;
        }
    }
}