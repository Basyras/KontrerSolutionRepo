using Basyc.DomainDrivenDesign.Application;
using Basyc.MessageBus.Client.RequestResponse;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.ConsoleApp
{
    public class CustomerCreatedEventHandler : IEventHandler<CustomerCreatedEvent>
    {
        private readonly ILogger<CustomerCreatedEventHandler> logger;

        public CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger)
        {
            this.logger = logger;
        }
        public Task Handle(CustomerCreatedEvent message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation($"Handeling message {message.GetType().FullName}");
            logger.LogInformation($"Handeled message {message.GetType().FullName}");

            return Task.CompletedTask;
            
        }
    }
}
