using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.NetMQ.Shared;

public record DeserializedMessageResult(int SessionId, bool IsResponse, bool ExpectsResponse, object Message, Type MessageType, Type? ResponseType);
