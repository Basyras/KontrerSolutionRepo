using System;

namespace Basyc.MessageBus.Manager.Application
{
    public class DefaultMessageDomainExplorerOptions
    {
        public Type IQueryType { get; set; }
        public Type ICommandType { get; set; }
        public Type ICommandWithResponseType { get; set; }
    }
}