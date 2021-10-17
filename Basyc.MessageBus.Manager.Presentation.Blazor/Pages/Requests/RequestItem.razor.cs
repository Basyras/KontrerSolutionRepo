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
                //ParameterValues = new List<string>(Request.Parameters.Count);
                ParameterValues = Enumerable.Range(0, Request.Parameters.Count).Select(x => string.Empty).ToList();
                Enumerable.Range(0, Request.Parameters.Count).ToList().ForEach(x => SetParamDefaultValue(x));
                //foreach (var paramInfo in Request.Parameters)
                //{
                //    var defaultValue = paramInfo.Type.GetDefaultValue();
                //    string stringValue = null;
                //    if(defaultValue == null)
                //    {
                //        stringValue = "@null";
                //    }
                //    else if(paramInfo.Type.IsPrimitive)
                //    {
                //        stringValue = defaultValue.ToString();                        
                //    }
                //    else
                //    {
                //        stringValue = JsonSerializer.Serialize(defaultValue);
                //    }

                //    ParameterValues.Add(stringValue);
                //}


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

        private void SetParamDefaultValue(int index)
        {
            var paramType = request.Parameters[index].Type;
            if (paramType.IsValueType)
            {
                ParameterValues[index] = paramType.GetDefaultValue().ToString();
            }
            else if(paramType == typeof(string))
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