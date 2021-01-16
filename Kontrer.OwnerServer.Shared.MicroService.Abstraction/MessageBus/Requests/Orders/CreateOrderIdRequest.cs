using Kontrer.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.Requests.Orders
{
    public class CreateOrderIdRequest : IRequest<int>
    {
        public CreateOrderIdRequest()
        {
            
        }

    
    }
}
