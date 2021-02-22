using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.Requests
{
    public abstract class GenericResponseBase<TResponse>
    {
        public TResponse Data { get; }

        public GenericResponseBase(TResponse response)
        {
            Data = response;
        }
        
    }
}
