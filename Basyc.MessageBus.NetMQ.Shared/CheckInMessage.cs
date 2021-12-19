using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.NetMQ.Shared
{
    public record CheckInMessage(string clientIdentity, int handlers);
}
