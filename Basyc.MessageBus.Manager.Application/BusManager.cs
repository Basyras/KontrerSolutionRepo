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
    public class BusManager : IBusManager
    {
        private readonly IDomainInfoProvider messageDomainLoader;
        public IReadOnlyList<DomainInfo> DomainInfos { get; private set; }

        public bool Loaded { get; private set; }

        public BusManager(IDomainInfoProvider messageDomainLoader)
        {
            this.messageDomainLoader = messageDomainLoader;
        }

        public void Load()
        {
            DomainInfos = messageDomainLoader.GetDomainInfos();
            Loaded = true;
        }
    }
}