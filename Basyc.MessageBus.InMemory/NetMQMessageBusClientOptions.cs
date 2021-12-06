using Basyc.MessageBus.Client.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.NetMQ
{
    public class NetMQMessageBusClientOptions
    {
        public List<MessageHandlerInfo> Handlers { get; } = new List<MessageHandlerInfo>();        
        /// <summary>
        /// Have and can use register handlers to handle messages, if yes, socket for listening is initialized;
        /// </summary>
        public bool IsConsumerOfMessages { get => Handlers.Count > 0; }
    }
}
