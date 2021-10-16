using System.Collections.Generic;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application
{
    public interface IMessagesExplorerManager
    {
        List<MessageDomainInfo> Domains { get; }

        void Initialize(params Assembly[] assemblies);
    }
}