using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.HttpProxy.Client
{
    public class MessageBusHttpProxyClientOptions
    {
        public MessageBusHttpProxyClientOptions()
        {
        }

        public Uri ProxyHostUri { get; set; }
    }
}