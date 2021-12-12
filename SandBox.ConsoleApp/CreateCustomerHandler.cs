using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.ConsoleApp
{
    public class CreateCustomerHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        private readonly ILogger<CreateCustomerHandler> logger;

        public CreateCustomerHandler(ILogger<CreateCustomerHandler> logger)
        {
            this.logger = logger;
        }
        public Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation($"Handeling message {message.GetType().FullName}");
            logger.LogInformation($"Handeled message {message.GetType().FullName}");
            var newCustomer = new CustomerEntity()
            {
                Email = message.Email,
                FirstName = message.FirstName,
                SecondName = message.LastName,
                Id = new Random().Next()
            };
            return Task.FromResult(new CreateCustomerCommandResponse());
            
        }
    }
}
