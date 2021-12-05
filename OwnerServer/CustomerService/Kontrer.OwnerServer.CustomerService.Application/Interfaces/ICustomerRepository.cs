using Basyc.Repositories;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Application.Interfaces
{
    public interface ICustomerRepository : IAsyncInstantCrudRepository<CustomerEntity, int>
    {
        Task<List<CustomerEntity>> GetByIdsAsync(IEnumerable<int> ids);
    }
}