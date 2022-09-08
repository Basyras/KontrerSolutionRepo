﻿using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.Logging;

namespace SandBox.ConsoleApp
{
	public class DeleteCustomerHandler : ICommandHandler<DeleteCustomerCommand>
	{
		private readonly ILogger<CreateCustomerHandler> logger;

		public DeleteCustomerHandler(ILogger<CreateCustomerHandler> logger)
		{
			this.logger = logger;
		}
		public Task Handle(DeleteCustomerCommand message, CancellationToken cancellationToken = default)
		{
			logger.LogInformation($"Handeling message {message.GetType().FullName}");
			logger.LogInformation($"Handeled message {message.GetType().FullName}");
			return Task.CompletedTask;
		}
	}
}