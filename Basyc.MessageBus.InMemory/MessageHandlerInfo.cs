using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.NetMQ
{
    public class MessageHandlerInfo
    {
        public MessageHandlerInfo(Type handlerType, Type messageType, MethodInfo handleMethod)
        {
            HandlerType = handlerType;
            MessageType = messageType;
            HandleMethod = handleMethod;
        }

        public MessageHandlerInfo(Type handlerType, Type messageType, Type responseType, MethodInfo handleMethod)
        {
            HandlerType = handlerType;
            MessageType = messageType;
            ResponseType = responseType;
            HandleMethod = handleMethod;
        }

        public Type MessageType { get; }
        public Type? ResponseType { get; }
        public bool HasResponse => ResponseType is not null;
        public MethodInfo HandleMethod { get; }
        public Type HandlerType { get; }
    }
}
