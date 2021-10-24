using System;
using System.Collections.Generic;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Infrastructure
{
    public class TypedDomainProviderOptions
    {
        public Type IQueryType { get; set; }

        public Type ICommandType { get; set; }
        public Type ICommandWithResponseType { get; set; }

        public Type IMessageType { get; set; }
        public Type IMessageWithResponseType { get; set; }

        public List<Assembly> AssembliesToScan { get; set; }
    }
}