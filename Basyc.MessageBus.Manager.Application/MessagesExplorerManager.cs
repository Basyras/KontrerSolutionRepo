using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public class MessagesExplorerManager : IMessagesExplorerManager
    {
        private readonly IMessageExplorer messageDomainLoader;
        public List<MessageDomainInfo> Domains { get; private set; }

        public MessagesExplorerManager(IMessageExplorer messageDomainLoader)
        {
            this.messageDomainLoader = messageDomainLoader;
        }

        public void Initialize(params Assembly[] assemblies)
        {
            Domains = messageDomainLoader.FindMessageDomains(assemblies);
        }
    }
}