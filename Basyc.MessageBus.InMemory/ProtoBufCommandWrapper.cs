using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Basyc.MessageBus.Client.NetMQ;

[ProtoContract]
public class ProtoBufCommandWrapper
{
    //Supressing warning since this ctor is only used for serializers
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected ProtoBufCommandWrapper()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }

    public ProtoBufCommandWrapper(string commandAssemblyQualifiedName, byte[] commandBytes, int communicationId, bool isResponse)
    {
        CommandAssemblyQualifiedName = commandAssemblyQualifiedName;
        CommandBytes = commandBytes;
        SessionId = communicationId;
        IsResponse = isResponse;
    }

    [ProtoMember(1)]
    public int SessionId { get; set; }
    [ProtoMember(2)]
    public bool IsResponse { get; set; }

    [ProtoMember(3)]
    public string? CommandAssemblyQualifiedName { get; set; }

    [ProtoMember(4)]
    public byte[] CommandBytes { get; set; }
}
