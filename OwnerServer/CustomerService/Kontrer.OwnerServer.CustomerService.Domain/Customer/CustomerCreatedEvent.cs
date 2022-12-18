using Basyc.MessageBus.Shared;

namespace Kontrer.OwnerServer.CustomerService.Domain.Customer
{
	public record CustomerCreatedEvent(CustomerEntity CreatedCustomer) : IEvent;
}
