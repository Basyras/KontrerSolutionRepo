using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.HttpProxy.Shared
{
    //public record ProxyRequest(string RequestType, string RequestResponseType = null, string Request = null)
    //{
    //}

    public class ProxyRequest
    {
        public static ProxyRequest Create(Type requestType, string requestJson, Type responseType = null)
        {
            string responseTypeString = responseType == null ? null : responseType.AssemblyQualifiedName;
            return new ProxyRequest(requestType.AssemblyQualifiedName, responseTypeString, requestJson);
        }

        [Obsolete("This constructor is public only for serializer. Use static ProxyRequest.Create instead")]
        public ProxyRequest(string requestAssemblyQualifiedTypeName, string responseAssemblyQualifiedTypeName, string requestJson = null)
            : this(requestAssemblyQualifiedTypeName, requestJson)
        {
            ResponseAssemblyQualifiedTypeName = responseAssemblyQualifiedTypeName;
        }

        private ProxyRequest(string requestAssemblyQualifiedTypeName, string requestJson = null)
        {
            RequestAssemblyQualifiedTypeName = requestAssemblyQualifiedTypeName;
            RequestJson = requestJson;
        }

        //private ProxyRequest(string requestType, string requestResponseType = null, string request = null)
        //{
        //    RequestType = requestType;
        //    RequestResponseType = requestResponseType;
        //    Request = request;
        //}

        public string RequestAssemblyQualifiedTypeName { get; init; }
        public string ResponseAssemblyQualifiedTypeName { get; init; }
        public string RequestJson { get; init; }
    }
}