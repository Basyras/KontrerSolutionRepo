using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse.Orders
{
    public class CreateOrderIdRequest : IRequest<CreateOrderIdResponse>
    {        
    }
}
