using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests
{
    public partial class RequestDomainItem
    {
        [Parameter]
        public DomainInfoViewModel Domain { get; set; }

        [Parameter]
        public EventHandler<RequestItem> OnMessageSending { get; set; }
    }
}