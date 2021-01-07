using Kontrer.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.Requests.Customers
{
    public class TryCreateCustomerRequest : IRequest<CustomerModel>
    {
        public TryCreateCustomerRequest(CustomerModel customer)
        {
            Customer = customer;
        }

        public CustomerModel Customer { get; }
    }
}
