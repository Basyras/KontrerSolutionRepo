using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.NetMQ
{
    public class TypedMessageHandlerInfo
    {
        public TypedMessageHandlerInfo(Type handlerType, Type messageType, MethodInfo handleMethod)
        {
            HandlerType = handlerType;
            MessageType = messageType;
            HandleMethodInfo = handleMethod;
        }

        public TypedMessageHandlerInfo(Type handlerType, Type messageType, Type responseType, MethodInfo handleMethod)
        {
            HandlerType = handlerType;
            MessageType = messageType;
            ResponseType = responseType;
            HandleMethodInfo = handleMethod;
        }

        public Type MessageType { get; }
        public Type? ResponseType { get; }
        public bool HasResponse => ResponseType is not null;
        public MethodInfo HandleMethodInfo { get; }
        public Type HandlerType { get; }
    }
}
