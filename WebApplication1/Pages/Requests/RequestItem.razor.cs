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
using Basyc.MessageBus.Manager.Application.Initialization;

namespace Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests
{
    public partial class RequestItem
    {
        private RequestItemViewModel requestItemViewModel;

        [Parameter]
        public EventCallback OnMessageSending { get; set; }

        [Parameter]
        public EventCallback<string> OnValueChanged { get; set; }

        [Parameter]
        public RequestItemViewModel RequestItemViewModel { get => requestItemViewModel; set => requestItemViewModel = value; }

        public async Task SendMessage(RequestInfo request)
        {
            RequestItemViewModel.IsLoading = true;
            await OnMessageSending.InvokeAsync(this);
            RequestItemViewModel.IsLoading = false;
        }

        //private async void SetParamValue(int index, object value)
        //{
        //    string stringValue = value.ToString();
        //    var paramInfo = RequestItemViewModel.RequestInfo.Parameters[index];
        //    if (paramInfo.Type.IsValueType)
        //    {
        //    }

        //    if (stringValue == string.Empty)
        //    {
        //        await Task.Delay(1);
        //        SetParamDefaultValue(index);
        //    }
        //    else
        //    {
        //        RequestItemViewModel.ParameterValues[index] = stringValue;
        //    }
        //}

        //private void SetParamDefaultValue(int index)
        //{
        //    var paramType = RequestItemViewModel.RequestInfo.Parameters[index].Type;
        //    var defaultString = GetDefaultValueString(paramType);
        //    RequestItemViewModel.ParameterValues[index] = defaultString;
        //}

        private string GetDefaultValueString(Type type)
        {
            if (type.IsValueType)
            {
                return type.GetDefaultValue().ToString();
            }
            else if (type == typeof(string))
            {
                return string.Empty;
            }
            else
            {
                return "@null";
            }
        }

        protected override void OnInitialized()
        {
            RequestItemViewModel.ParameterValues.CollectionChanged += ParameterValues_CollectionChanged;
            //Enumerable.Range(0, RequestItemViewModel.RequestInfo.Parameters.Count).ToList().ForEach(x => SetParamDefaultValue(x));
            for (int paramIndex = 0; paramIndex < RequestItemViewModel.RequestInfo.Parameters.Count; paramIndex++)
            {
                var defaultValue = GetDefaultValueString(RequestItemViewModel.RequestInfo.Parameters[paramIndex].Type);
                RequestItemViewModel.ParameterValues[paramIndex] = defaultValue;
            }
            base.OnInitialized();
        }

        private void ParameterValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var newValue = (string)e.NewItems[0];
            var defaultValue = GetDefaultValueString(RequestItemViewModel.RequestInfo.Parameters[e.NewStartingIndex].Type);
            if (newValue == string.Empty && newValue != defaultValue)
            {
                //SetParamDefaultValue(e.NewStartingIndex);
                RequestItemViewModel.ParameterValues[e.NewStartingIndex] = defaultValue;
            }
        }
    }
}