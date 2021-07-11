using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.Shared.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Application.Customers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly IMessageBusManager bus;

        public CustomerManager(IMessageBusManager bus)
        {
            this.bus = bus;
        }

        public async ValueTask<CustomerEntity> CreateCustomer(string firstName, string lastName, string email)
        {
            var response = await bus.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new(firstName, lastName, email));
            return response.NewCustomer;
        }

        public async Task DeleteCustomer(int customerId)
        {
            await bus.SendAsync<DeleteCustomerCommand>(new DeleteCustomerCommand(customerId));
        }

        public async ValueTask<List<CustomerEntity>> GetCustomers()
        {
            var response = await bus.RequestAsync<GetCustomersQuery, GetCustomersQueryResponse>(new(new int[0]));
            return response.Customers;
        }

        public async ValueTask<List<CustomerEntity>> GetCustomers(int[] customerIds)
        {
            var response = await bus.RequestAsync<GetCustomersQuery, GetCustomersQueryResponse>(new(customerIds));
            return response.Customers;
        }
    }
}