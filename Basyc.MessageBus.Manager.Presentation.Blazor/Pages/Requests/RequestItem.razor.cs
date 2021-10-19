using Basyc.MessageBus.Manager.Application;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Kontrer.Shared.Helpers;

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
                Enumerable.Range(0, Request.Parameters.Count).ToList().ForEach(x => SetParamDefaultValue(x));
            }
        }

        [Parameter]
        public EventHandler OnMessageSending { get; set; }

        public ResponseViewModel Response { get; set; } = new ResponseViewModel(string.Empty, ResponseType.NoResponse, false, string.Empty);

        public List<string> ParameterValues { get; private set; }

        public void SendMessage(RequestInfo message)
        {
            OnMessageSending?.Invoke(this, EventArgs.Empty);
        }

        private void SetParamValue(int index, object value)
        {
            string stringValue = value.ToString();
            var paramInfo = Request.Parameters[index];
            if (paramInfo.Type.IsValueType)
            {
                if (stringValue == string.Empty)
                {
                    stringValue = paramInfo.Type.GetDefaultValue().ToString();
                }
            }
            ParameterValues[index] = stringValue;
            //OnParametersSet();
        }

        private void SetParamDefaultValue(int index)
        {
            var paramType = request.Parameters[index].Type;
            if (paramType.IsValueType)
            {
                ParameterValues[index] = paramType.GetDefaultValue().ToString();
            }
            else if (paramType == typeof(string))
            {
                ParameterValues[index] = string.Empty;
            }
            else
            {
                ParameterValues[index] = "@null";
            }
        }
    }
}