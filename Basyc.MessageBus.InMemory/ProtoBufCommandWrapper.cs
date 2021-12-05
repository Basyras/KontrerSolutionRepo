using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Basyc.MessageBus.InMemory;

[ProtoContract]
public class ProtoBufCommandWrapper
{
    protected ProtoBufCommandWrapper()
    {

    }

    public ProtoBufCommandWrapper(string commandAssemblyQualifiedName, byte[] commandBytes, int communicationId)
    {
        CommandAssemblyQualifiedName = commandAssemblyQualifiedName;
        CommandBytes = commandBytes;
        CommunicationId = communicationId;
    }

    [ProtoMember(1)]
    public int CommunicationId { get; set; }

    [ProtoMember(2)]
    public string? CommandAssemblyQualifiedName { get; set; }
    [ProtoMember(3)]
    public byte[] CommandBytes { get; set; }
}
