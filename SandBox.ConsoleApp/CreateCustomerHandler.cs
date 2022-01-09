﻿using Basyc.DomainDrivenDesign.Application;
using Basyc.MessageBus.Client;
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
        private readonly ITypedMessageBusClient busClient;
        private readonly ILogger<CreateCustomerHandler> logger;

        public CreateCustomerHandler(ITypedMessageBusClient busClient, ILogger<CreateCustomerHandler> logger)
        {
            this.busClient = busClient;
            this.logger = logger;
        }
        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand message, CancellationToken cancellationToken = default)
        {
            logger.LogDebug($"Handeling message {message.GetType().FullName}");
            CustomerEntity? newCustomer = new CustomerEntity()
            {
                Email = message.Email,
                FirstName = message.FirstName,
                SecondName = message.LastName,
                Id = new Random().Next()
            };
            logger.LogDebug($"Handeled message {message.GetType().FullName}");

            await busClient.PublishAsync(new CustomerCreatedEvent(newCustomer));
            return new CreateCustomerCommandResponse(newCustomer);
            
        }
    }
}
