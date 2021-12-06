using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.NetMQ
{
    public record DeserializedMessageResult(int SessionId, bool ExpectsResponse, object Message, Type MessageType);
}
