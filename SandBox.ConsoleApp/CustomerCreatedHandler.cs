using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.Extensions.Logging;

namespace SandBox.ConsoleApp
{
	public partial class CustomerCreatedHandler : IEventHandler<CustomerCreatedEvent>
	{
		private readonly ILogger<CustomerCreatedHandler> logger;

		public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
		{
			this.logger = logger;
		}
		public async Task Handle(CustomerCreatedEvent message, CancellationToken cancellationToken = default)
		{
			LogEventReceived();
			await Task.Delay(250);
			LogEventHandeled();
		}

		[LoggerMessage(0, LogLevel.Information, "Event CustomerCreatedHandler received")]
		partial void LogEventReceived();


		[LoggerMessage(1, LogLevel.Information, "Event CustomerCreatedHandler handeled")]
		partial void LogEventHandeled();
	}
}
