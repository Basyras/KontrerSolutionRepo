using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Shared
{
    [ProtoContract]
    public class ProxyResponse
    {
        protected ProxyResponse()
        {

        }

        public ProxyResponse(string messageType, bool hasResponse, byte[] responseData = null, string responseType = null)
        {
            MessageType = messageType;
            HasResponse = hasResponse;
            ResponseBytes = responseData ?? Array.Empty<byte>();
            ResponseType = responseType;
        }

        [ProtoMember(1)]
        public string MessageType { get; set; }
		/// <summary>
		/// Is request/query/message that expects reponse message back.
		/// </summary>
		[ProtoMember(4)]
        public bool HasResponse { get; set; }
        [ProtoMember(2)]
        public byte[] ResponseBytes { get; set; }
        [ProtoMember(3)]
        public string ResponseType { get; set; }

    }
}
