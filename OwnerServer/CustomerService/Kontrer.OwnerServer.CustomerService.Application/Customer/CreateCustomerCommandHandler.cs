using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.CustomerService.Application.Interfaces;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Application.Customer
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        private readonly ICustomerRepository repository;

        public CreateCustomerCommandHandler(ICustomerRepository repository)
        {
            this.repository = repository;
        }

        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
        {
            var newCustomer = new CustomerEntity();
            newCustomer.FirstName = command.FirstName;
            newCustomer.SecondName = command.LastName;
            newCustomer.Email = command.Email;
            newCustomer = await repository.InstaAddAsync(newCustomer);
            return new CreateCustomerCommandResponse(newCustomer);
        }
    }
}