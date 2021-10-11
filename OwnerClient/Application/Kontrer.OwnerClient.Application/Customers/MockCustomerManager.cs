using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Application.Customers
{
    //Exists only for designing ui
    public class MockCustomerManager : ICustomerManager
    {
        private List<CustomerEntity> customers = new List<CustomerEntity>();

        public ValueTask<CustomerEntity> CreateCustomer(string firstName, string lastName, string email)
        {
            var newCustomer = new CustomerEntity();
            newCustomer.FirstName = firstName;
            newCustomer.SecondName = lastName;
            newCustomer.Email = email;
            newCustomer.Id = new Random().Next();
            customers.Add(newCustomer);
            return ValueTask.FromResult(newCustomer);
        }

        public Task DeleteCustomer(int customerId)
        {
            customers.Remove(customers.First(x => x.Id == customerId));
            return Task.CompletedTask;
        }

        public ValueTask<List<CustomerEntity>> GetCustomers()
        {
            return ValueTask.FromResult(customers);
        }

        public ValueTask<List<CustomerEntity>> GetCustomers(int[] customerIds)
        {
            return ValueTask.FromResult(customers.Where(x => customerIds.Contains(x.Id)).ToList());
        }
    }
}