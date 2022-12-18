using Basyc.DomainDrivenDesign.Application;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Application.Customer
{
	public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, GetCustomersQueryResponse>
	{
		private readonly ICustomerRepository repository;

		public GetCustomersQueryHandler(ICustomerRepository repository)
		{
			this.repository = repository;
		}

		public async Task<GetCustomersQueryResponse> Handle(GetCustomersQuery command, CancellationToken cancellationToken = default)
		{
			List<CustomerEntity> customers;
			if (command.CustomerIds == null || command.CustomerIds.Count() == 0)
			{
				customers = (await repository.GetAllAsync()).Values.ToList();
			}
			else
			{
				customers = await repository.GetByIdsAsync(command.CustomerIds);
			}
			var response = new GetCustomersQueryResponse(customers);
			return response;
		}
	}
}