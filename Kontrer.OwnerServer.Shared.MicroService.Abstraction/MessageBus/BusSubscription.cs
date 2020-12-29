using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus
{
    public record BusSubscription(string Topic,Type RequestType, System.Delegate Handler);
    
}
