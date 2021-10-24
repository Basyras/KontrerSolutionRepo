using Basyc.MessageBus.Manager.Application.Initialization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public class MessageManager : IMessageManager
    {
        private readonly IDomainProvider messageDomainLoader;
        public IReadOnlyList<DomainInfo> DomainInfos { get; private set; }

        public MessageManager(IDomainProvider messageDomainLoader)
        {
            this.messageDomainLoader = messageDomainLoader;
        }

        public void Initialize()
        {
            DomainInfos = messageDomainLoader.GetDomains();
        }
    }
}