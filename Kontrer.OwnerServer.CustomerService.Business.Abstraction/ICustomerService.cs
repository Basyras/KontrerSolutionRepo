using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Business.Abstraction
{
    public interface ICustomerService
    {
        Task<Dictionary<int, CustomerModel>> GetAllCustomersAsync();
        Task<CustomerModel> GetCustomerAsync(int customerId);
        Task CreateCustomerAsync(CustomerModel newCustomer);
        Task DeleteCustomerAsync(int customerId);
        Task ChangeCustomerDetailsAsync(CustomerModel updatedCustomer);
        

        Task CreateAccommodation(int customerId, FinishedAccommodationModel accommodationModel);





    }
}
