using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public record MessageDomainInfo(string DomainName, List<RequestInfo> Messages);
}