using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Pages.Requests;

public class RequestItemViewModel
{
    public RequestItemViewModel(RequestInfo requestInfo) : this(null, requestInfo)
    {
    }

    public RequestItemViewModel(RequestResult response, RequestInfo requestInfo)
    {
        Response = response;
        RequestInfo = requestInfo;
        ParameterValues = new ObservableCollection<string>(Enumerable.Range(0, RequestInfo.Parameters.Count).Select(x => string.Empty).ToList());
    }

    public RequestResult Response { get; set; }
    public RequestInfo RequestInfo { get; init; }
    public ObservableCollection<string> ParameterValues { get; init; }
    public bool IsLoading { get; set; }
}
