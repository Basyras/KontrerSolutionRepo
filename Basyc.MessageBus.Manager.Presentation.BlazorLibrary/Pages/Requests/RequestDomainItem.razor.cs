using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Pages.Requests;

public partial class RequestDomainItem
{
    [Parameter]
    public DomainItemViewModel DomainItemViewModel { get; set; }

    [Parameter]
    public EventCallback<RequestItem> OnMessageSending { get; set; }
}
