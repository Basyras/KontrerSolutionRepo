using Basyc.DomainDrivenDesign.Application;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.ConsoleApp
{
    public class CreateCustomerHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        private readonly ILogger<CreateCustomerHandler> logger;

        public CreateCustomerHandler(ILogger<CreateCustomerHandler> logger)
        {
            this.logger = logger;
        }
        public Task<CreateCustomerResponse> Handle(CreateCustomerCommand message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation($"Handeling message {message.GetType().FullName}");
            logger.LogInformation($"Handeled message {message.GetType().FullName}");
            return Task.FromResult(new CreateCustomerResponse(message.Name));
        }
    }
}
