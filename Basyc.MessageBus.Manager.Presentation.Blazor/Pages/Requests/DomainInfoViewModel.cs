using Basyc.MessageBus.Manager.Application;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests
{
    public class DomainInfoViewModel
    {
        public DomainInfoViewModel(MessageDomainInfo messageDomainInfo, List<RequestInfoViewModel> messages)
        {
            MessageDomainInfo = messageDomainInfo;
            Messages = messages;
        }

        public MessageDomainInfo MessageDomainInfo { get; }
        public List<RequestInfoViewModel> Messages { get; }
    }
}