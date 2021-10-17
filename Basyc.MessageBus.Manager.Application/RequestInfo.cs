using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application
{
    public class RequestInfo
    {
        public RequestInfo(Type type, bool isCommand, List<RequestParameterInfo> parameters, Type responseType, string requestDisplayName)
        {
            Type = type;
            Parameters = parameters;
            HasResponse = true;
            ResponseType = responseType;
            IsCommand = isCommand;
            RequestDisplayName = requestDisplayName;
        }

        public RequestInfo(Type type, bool isCommand, List<RequestParameterInfo> parameters, string requestDisplayName)
        {
            Type = type;
            Parameters = parameters;
            HasResponse = false;
            ResponseType = null;
            IsCommand = isCommand;
            RequestDisplayName = requestDisplayName;
        }

        public Type Type { get; }
        public string RequestDisplayName { get; }
        public bool IsCommand { get; }
        public List<RequestParameterInfo> Parameters { get; }
        public bool HasResponse { get; }
        public Type ResponseType { get; }
    }
}