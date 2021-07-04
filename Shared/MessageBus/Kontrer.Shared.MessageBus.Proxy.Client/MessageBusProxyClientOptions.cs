using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.Proxy.Client
{
    public class MessageBusProxyClientOptions
    {
        public MessageBusProxyClientOptions()
        {
        }

        public Uri ProxyHostUri { get; set; }
    }
}