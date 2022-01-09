using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basyc.MessageBus.Client;

namespace Kontrer.OwnerClient.Application.Customers
{
    public class BusCustomerManager : ICustomerManager
    {
        private readonly ITypedMessageBusClient bus;

        public BusCustomerManager(ITypedMessageBusClient bus)
        {
            this.bus = bus;
        }

        public async ValueTask<CustomerEntity> CreateCustomer(string firstName, string lastName, string email)
        {
            var response = await bus.RequestAsync<CreateCustomerCommand, CreateCustomerCommandResponse>(new(firstName, lastName, email));
            //return response.;
            return null;
        }

        public async Task DeleteCustomer(int customerId)
        {
            await bus.SendAsync<DeleteCustomerCommand>(new DeleteCustomerCommand(customerId));
        }

        public async ValueTask<List<CustomerEntity>> GetCustomers()
        {
            var response = await bus.RequestAsync<GetCustomersQuery, GetCustomersQueryResponse>(new(new int[0]));
            return response.AsT0.Customers;
        }

        public async ValueTask<List<CustomerEntity>> GetCustomers(int[] customerIds)
        {
            var response = await bus.RequestAsync<GetCustomersQuery, GetCustomersQueryResponse>(new(customerIds));
            return response.AsT0.Customers;
        }
    }
}