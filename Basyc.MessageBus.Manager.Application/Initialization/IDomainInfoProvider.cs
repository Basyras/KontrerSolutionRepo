using System;
using System.Collections.Generic;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application.Initialization
{
    public interface IDomainInfoProvider
    {
        List<DomainInfo> GetDomainInfos();
    }
}