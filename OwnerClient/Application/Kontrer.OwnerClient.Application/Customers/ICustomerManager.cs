using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Application.Customers
{
    public interface ICustomerManager
    {
        ValueTask<List<CustomerEntity>> GetCustomers();

        ValueTask<List<CustomerEntity>> GetCustomers(int[] customerIds);

        Task DeleteCustomer(int customerId);

        ValueTask<CustomerEntity> CreateCustomer(string firstName, string lastName, string email);
    }
}