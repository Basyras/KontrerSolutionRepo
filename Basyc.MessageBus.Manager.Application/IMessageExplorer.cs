using System;
using System.Collections.Generic;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application
{
    public interface IMessageExplorer
    {
        List<MessageDomainInfo> FindMessageDomains(params Assembly[] assembliesWithMessages);
    }
}