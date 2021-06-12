using Dapr.Client;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse.Customers;
using Kontrer.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Dapr.MessageBus.Handlers.Customers
{
    public class TryCreateCustomerHandler : DaprRequestHandler<TryCreateCustomerRequest,CustomerModel>
    {
        public TryCreateCustomerHandler(DaprClient daprClient) : base(daprClient, AppNames.CustomerService)
        {
            
        }
    }
}
