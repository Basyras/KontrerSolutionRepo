using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Application.Customer
{
	public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
	{
		private readonly ICustomerRepository repository;

		public DeleteCustomerCommandHandler(ICustomerRepository repository)
		{
			this.repository = repository;
		}

		public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
		{
			await repository.InstaRemoveAsync(command.CustomerId);
		}
	}
}