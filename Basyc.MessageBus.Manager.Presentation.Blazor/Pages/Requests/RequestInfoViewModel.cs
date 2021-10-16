using Basyc.MessageBus.Manager.Application;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests
{
    public class RequestInfoViewModel
    {
        public RequestInfoViewModel(RequestInfo requestInfo)
        {
            RequestInfo = requestInfo;
            ParameterValues = Enumerable.Range(0, requestInfo.Parameters.Count).Select(x => string.Empty).ToList();
        }

        public RequestInfo RequestInfo { get; }
        public List<string> ParameterValues { get; }
    }
}