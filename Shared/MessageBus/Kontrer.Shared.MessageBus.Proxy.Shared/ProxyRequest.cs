using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Shared
{
    [ProtoContract]
    public class ProxyRequest
    {        
        protected ProxyRequest()
        {

        }

        public ProxyRequest(string requestType, byte[] requestData = null, string responseType = null)             
        {
            MessageType = requestType;
            RequestData = requestData ?? Array.Empty<byte>();
            ResponseAssemblyQualifiedTypeName = responseType;
        }

        [ProtoMember(1)]
        public string MessageType { get; set; }
        [ProtoMember(2)]
        public byte[] RequestData { get; set; }
        [ProtoMember(3)]
        public string ResponseAssemblyQualifiedTypeName { get; set; }
    }
}