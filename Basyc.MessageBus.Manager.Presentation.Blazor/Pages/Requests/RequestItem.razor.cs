using Basyc.MessageBus.Manager.Application;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests
{
    public partial class RequestItem
    {
        private RequestInfo request;

        [Parameter]
        public RequestInfo Request
        {
            get => request;
            set
            {
                request = value;
                ParameterValues = Enumerable.Range(0, Request.Parameters.Count).Select(x => string.Empty).ToList();
            }
        }

        [Parameter]
        public EventHandler OnMessageSending { get; set; }

        public List<string> ParameterValues { get; private set; }

        public void SendMessage(RequestInfo message)
        {
            OnMessageSending?.Invoke(this, EventArgs.Empty);
        }

        private void SetParamValue(int index, string value)
        {
            ParameterValues[index] = value;
        }
    }
}