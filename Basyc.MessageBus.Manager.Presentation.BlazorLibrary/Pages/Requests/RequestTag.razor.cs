using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Pages.Requests;

public partial class RequestTag
{
    private RequestTagType requestType;

    public string Content { get; private set; }

    [Parameter]
    public RequestTagType RequestType
    {
        get => requestType;
        set
        {
            requestType = value;
            Content = requestType == RequestTagType.Generic ? "message" : requestType.ToString().ToLower();
        }
    }
}
