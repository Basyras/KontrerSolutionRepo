using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.Logging;

namespace SandBox.ConsoleApp
{
	public partial class DeleteCustomerHandler : ICommandHandler<DeleteCustomerCommand>
	{
		private readonly ILogger<CreateCustomerHandler> logger;

		public DeleteCustomerHandler(ILogger<CreateCustomerHandler> logger)
		{
			this.logger = logger;
		}
		public Task Handle(DeleteCustomerCommand message, CancellationToken cancellationToken = default)
		{
			LogHandleStart(message.GetType().FullName!);
			LogHandleEnd(message.GetType().FullName!);
			return Task.CompletedTask;
		}

		[LoggerMessage(0, LogLevel.Information, "Handle {messageName} started")]
		partial void LogHandleStart(string messageName);


		[LoggerMessage(1, LogLevel.Information, "Handle {messageName} finished")]
		partial void LogHandleEnd(string messageName);
	}
}
