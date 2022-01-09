using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Shared
{
    //public record ProxyRequest(string RequestType, string RequestResponseType = null, string Request = null)
    //{
    //}

    public class ProxyRequest
    {
        public static ProxyRequest Create(string requestType, byte[] requestData, Type responseType = null)
        {
            string responseTypeString = responseType == null ? null : responseType.AssemblyQualifiedName;
            //return new ProxyRequest(requestType.AssemblyQualifiedName, responseTypeString, requestJson);
#pragma warning disable CS0618 // Type or member is obsolete
            return new ProxyRequest(requestType, responseTypeString, requestData);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Obsolete("This constructor is public only for serializer. Use static ProxyRequest.Create instead")]
        public ProxyRequest(string requestAssemblyQualifiedTypeName, string responseAssemblyQualifiedTypeName, byte[] requestData = null)
            : this(requestAssemblyQualifiedTypeName, requestData)
        {
            ResponseAssemblyQualifiedTypeName = responseAssemblyQualifiedTypeName;
        }

        private ProxyRequest(string requestAssemblyQualifiedTypeName, byte[] requestData = null)
        {
            MessageType = requestAssemblyQualifiedTypeName;
            RequestData = requestData ?? Array.Empty<byte>();
        }

        //private ProxyRequest(string requestType, string requestResponseType = null, string request = null)
        //{
        //    RequestType = requestType;
        //    RequestResponseType = requestResponseType;
        //    Request = request;
        //}

        public string MessageType { get; init; }
        public string ResponseAssemblyQualifiedTypeName { get; init; }
        public byte[] RequestData { get; init; }
    }
}