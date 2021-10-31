using Basyc.MessageBus.Manager.Application.Initialization;
using System.Collections.Generic;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application
{
    public interface IMessageManager
    {
        IReadOnlyList<DomainInfo> DomainInfos { get; }
        bool Loaded { get; }

        void Load();
    }
}