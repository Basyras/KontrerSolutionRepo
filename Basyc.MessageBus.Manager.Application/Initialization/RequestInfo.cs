using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application.Initialization
{
    public class RequestInfo
    {
        public RequestInfo(RequestType requestType, IEnumerable<ParameterInfo> parameters, Type responseType, string requestDisplayName, string responseDisplayName)
            : this(requestType, parameters, requestDisplayName, true)
        {
            RequestType = requestType;
            ResponseDisplayName = responseDisplayName;
            ResponseType = responseType;
        }

        public RequestInfo(RequestType requestType, IEnumerable<ParameterInfo> parameters, string requestDisplayName)
            : this(requestType, parameters, requestDisplayName, false)
        {
        }

        private RequestInfo(RequestType requestType, IEnumerable<ParameterInfo> parameters, string requestDisplayName, bool hasResponse)
        {
            RequestType = requestType;
            Parameters = parameters.ToList();
            RequestDisplayName = requestDisplayName;
            HasResponse = hasResponse;
        }

        public string RequestDisplayName { get; init; }
        public RequestType RequestType { get; init; }
        public IReadOnlyList<ParameterInfo> Parameters { get; init; }
        public bool HasResponse { get; init; }
        public Type ResponseType { get; init; }
        public string ResponseDisplayName { get; init; } = string.Empty;
    }
}