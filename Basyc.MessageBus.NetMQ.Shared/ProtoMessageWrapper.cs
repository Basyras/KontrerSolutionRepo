using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Basyc.MessageBus.NetMQ.Shared;

[ProtoContract]
public class ProtoMessageWrapper
{
    //Supressing warning since this ctor is only used for serializers
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected ProtoMessageWrapper()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }

    public ProtoMessageWrapper(int sessionId, MessageCase messageCase, string messageType, byte[] messageData)
    {
        SessionId = sessionId;
        MessageCase = messageCase;        
        MessageType = messageType;
        MessageData = messageData;
    }

    [ProtoMember(1)]
    public int SessionId { get; set; }

    [ProtoMember(2)]
    public MessageCase MessageCase { get; }

    [ProtoMember(3)]
    public string MessageType { get; set; }

    [ProtoMember(4)]
    public byte[] MessageData { get; set; }
}
