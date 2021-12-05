using Basyc.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Domain.Customer
{
    public class GetCustomersQuery : IQuery<GetCustomersQueryResponse>
    {
        //public GetCustomersQuery()
        //{
        //}

        public GetCustomersQuery(int[] customerIds)
        {
            CustomerIds = customerIds;
        }

        public int[] CustomerIds { get; }
    }
}