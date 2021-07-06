using System.Collections.Generic;

namespace Kontrer.OwnerServer.CustomerService.Domain.Customer
{
    public record GetCustomersQueryResponse(List<CustomerEntity> Customers);
}