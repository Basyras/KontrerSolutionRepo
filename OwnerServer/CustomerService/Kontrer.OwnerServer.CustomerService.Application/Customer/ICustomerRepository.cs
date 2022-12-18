using Basyc.Repositories;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Application.Customer
{
	public interface ICustomerRepository : IAsyncInstantCrudRepository<CustomerEntity, int>
	{
		Task<List<CustomerEntity>> GetByIdsAsync(IEnumerable<int> ids);
	}
}