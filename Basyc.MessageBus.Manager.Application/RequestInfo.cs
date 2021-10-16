using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application
{
    public class RequestInfo
    {
        public RequestInfo(Type type, bool isCommand, List<RequestParameterInfo> parameters, Type responseType)
        {
            Type = type;
            Parameters = parameters;
            HasResponse = true;
            ResponseType = responseType;
            IsCommand = isCommand;
        }

        public RequestInfo(Type type, bool isCommand, List<RequestParameterInfo> parameters)
        {
            Type = type;
            Parameters = parameters;
            HasResponse = false;
            ResponseType = null;
            IsCommand = isCommand;
        }

        public Type Type { get; }
        public bool IsCommand { get; }
        public List<RequestParameterInfo> Parameters { get; }
        public bool HasResponse { get; }
        public Type ResponseType { get; }
    }
}