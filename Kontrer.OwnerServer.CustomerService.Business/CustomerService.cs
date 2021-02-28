using Kontrer.OwnerServer.CustomerService.Business.Abstraction;
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Accommodation;
using Kontrer.OwnerServer.CustomerService.Data.Abstraction.Customer;
using Kontrer.OwnerServer.IdGeneratorService.Presentation.Abstraction;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Business
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IAccommodationRepository accommodationRepository;
        private readonly IMessageBusManager messageBusManager;

        public CustomerService(ICustomerRepository customerRepository, IAccommodationRepository accommodationRepository, IMessageBusManager messageBusManager)
        {
            this.customerRepository = customerRepository;
            this.accommodationRepository = accommodationRepository;
            this.messageBusManager = messageBusManager;
        }

        public async Task CreateCustomerAsync(CustomerModel newCustomer)
        {
            var accommodationId = await messageBusManager.RequestAsync<CreateAccommodationIdRequest, int>();
            newCustomer.CustomerId = accommodationId;
            return customerRepository.AddAsync(newCustomer);
        }

        public Task DeleteCustomerAsync(int customerId)
        {
            return customerRepository.RemoveAsync(customerId);
        }

        public Task ChangeCustomerDetailsAsync(CustomerModel updatedCustomer)
        {
            return customerRepository.UpdateAsync(updatedCustomer);
        }

        public async Task CreateAccommodation(int customerId, FinishedAccommodationModel accommodationModel)
        {
            var customerExists = await customerRepository.CustomerExitsAsync(customerId);
            if (customerExists is false)
            {
                throw new InvalidOperationException($"Customer with id {customerId} does not exist");
            }

            await accommodationRepository.AddAsync(accommodationModel);
            
        }

        public Task<Dictionary<int, CustomerModel>> GetAllCustomersAsync()
        {
            return customerRepository.GetAllAsync();
        }

        public Task<CustomerModel> GetCustomerAsync(int customerId)
        {
            return customerRepository.TryGetAsync(customerId);
        }
    }
}
