using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customer
{
    public interface ICustomerRepository : IInstantCrudRepository<CustomerModel,int>, IPageRepository<CustomerModel>,IBulkRepository
    {
        Task<PageResult<CustomerModel>> GetPageByPatternAsync(int page, int itemsPerPage, string searchPattern);
        Task<bool> CustomerExitsAsync(int customerId);
    }
}
