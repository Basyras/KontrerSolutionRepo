using Kontrer.Shared.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Domain.Customer
{
    public class GetCustomersQuery : IQuery<GetCustomersQueryResponse>
    {
        public GetCustomersQuery()
        {
        }

        public GetCustomersQuery(List<int> customerIds)
        {
            CustomerIds = customerIds;
        }

        public List<int> CustomerIds { get; } = new List<int>();
    }
}