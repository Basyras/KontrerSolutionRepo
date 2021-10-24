using Basyc.MessageBus.Manager.Application.Initialization;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests
{
    public class DomainItemViewModel
    {
        public DomainItemViewModel(DomainInfo requestDomainInfo, IEnumerable<RequestItemViewModel> requestViewModels)
        {
            RequestDomainInfo = requestDomainInfo;
            RequestItemViewModels = new List<RequestItemViewModel>(requestViewModels);
        }

        public DomainInfo RequestDomainInfo { get; }
        public IReadOnlyCollection<RequestItemViewModel> RequestItemViewModels { get; }
    }
}