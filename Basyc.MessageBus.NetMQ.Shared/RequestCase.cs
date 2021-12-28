using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.NetMQ.Shared
{
    public record RequestCase(int SessionId, string RequestType, object RequestData, bool ExpectsResponse,Type? ResponseType);
}
