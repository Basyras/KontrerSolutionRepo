using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.Requests.Orders
{
    public class CreateOrderIdResponse : GenericResponseBase<int>
    {
        public CreateOrderIdResponse(int id) : base(id)
        {

        }
    }
}
