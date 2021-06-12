using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse.Orders
{
    public class CreateOrderIdResponse : GenericRequestBase<int>
    {
        public CreateOrderIdResponse(int id) : base(id)
        {

        }
    }
}
