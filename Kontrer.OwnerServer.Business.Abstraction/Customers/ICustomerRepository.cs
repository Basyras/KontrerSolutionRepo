using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Business.Abstraction.Customers
{
    public interface ICustomerRepository : IGenericRepository<CustomerModel, int>
    {
        Task<PageResult<CustomerModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern);


    }
}
