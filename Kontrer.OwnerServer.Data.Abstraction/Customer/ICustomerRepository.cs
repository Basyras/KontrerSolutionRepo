using Kontrer.OwnerServer.Data.Abstraction.Repositories;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Data.Abstraction.Customer
{
    public interface ICustomerRepository : IRepository
    {
        Task<Dictionary<int, CustomerModel>> GetAllAsync();
        Task<CustomerModel> GetAsync(int id);
        Task<PageResult<CustomerModel>> GetPageAsync(int page, int itemsPerPage, string searchedPattern);
        void Edit(CustomerModel model);
        void Remove(int id);
    }
}
