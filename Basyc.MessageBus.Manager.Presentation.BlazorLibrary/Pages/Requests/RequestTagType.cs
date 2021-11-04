using Basyc.MessageBus.Manager.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests
{
    public enum RequestTagType
    {
        Query,
        Command,
        Response,
        Generic,
    }

    public static class RequestTagTypeHelper
    {
        public static RequestTagType FromRequestType(RequestType requestType)
        {
            return requestType switch
            {
                RequestType.Query => RequestTagType.Query,
                RequestType.Command => RequestTagType.Command,
                RequestType.Generic => RequestTagType.Generic,
                _ => throw new NotImplementedException(),
            };
        }
    }
}