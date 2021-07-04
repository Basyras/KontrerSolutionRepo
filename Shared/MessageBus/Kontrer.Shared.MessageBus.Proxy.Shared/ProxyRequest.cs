using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.Proxy.Shared
{
    public record ProxyRequest(string RequestType, string RequestResponseType = null, string Request = null);
}