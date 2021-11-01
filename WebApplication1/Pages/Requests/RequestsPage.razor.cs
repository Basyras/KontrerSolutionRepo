using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests;
using Kontrer.Shared.Helpers;
using Kontrer.Shared.MessageBus;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Presentation.Blazor.Pages.Requests
{
    public partial class RequestsPage
    {
        [Inject]
        public IMessageManager Explorer { get; private set; }

        [Inject]
        public IMessageBusManager MessageBusManager { get; private set; }

        [Inject]
        public IDialogService DialogService { get; private set; }

        [Inject]
        public IRequestClient RequestClient { get; private set; }

        public List<DomainItemViewModel> DomainInfoViewModel { get; private set; } = new List<DomainItemViewModel>();

        protected override void OnInitialized()
        {
            if (Explorer.Loaded is false)
            {
                Explorer.Load();
            }

            DomainInfoViewModel = Explorer.DomainInfos
                .Select(domainInfo => new DomainItemViewModel(domainInfo, domainInfo.Requests
                    .Select(requestInfo => new RequestItemViewModel(requestInfo))
                    .OrderBy(x => x.RequestInfo.RequestType)))
                .ToList();

            base.OnInitialized();
        }

        public async Task SendMessage(RequestItem requestItem)
        {
            try
            {
                var requestInfo = requestItem.RequestItemViewModel.RequestInfo;
                List<Parameter> parameters = new List<Parameter>(requestInfo.Parameters.Count);
                for (int i = 0; i < requestInfo.Parameters.Count; i++)
                {
                    var paramInfo = requestInfo.Parameters[i];
                    var paramStringValue = requestItem.RequestItemViewModel.ParameterValues[i];
                    var castedParamValue = ParseParamInputValue(paramStringValue, paramInfo);
                    parameters.Add(new Parameter(paramInfo, castedParamValue));
                }

                var response = await RequestClient.TrySendRequest(new Request(requestInfo, parameters));
                requestItem.RequestItemViewModel.Response = response;
            }
            catch (Exception ex)
            {
                requestItem.RequestItemViewModel.Response = new RequestResult(true, ex.Message, default);
            }
        }

        private static object ParseParamInputValue(string paramStringValue, ParameterInfo parameterInfo)
        {
            if (paramStringValue == "@null")
            {
                return null;
            }

            if (paramStringValue == String.Empty)
            {
                return parameterInfo.Type.GetDefaultValue();
            }

            if (parameterInfo.Type == typeof(string))
            {
                return paramStringValue;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(parameterInfo.Type);
            object castedParam;
            if (converter.CanConvertFrom(typeof(string)))
            {
                castedParam = converter.ConvertFromInvariantString(paramStringValue);
                return castedParam;
            }

            TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(string));
            if (converter2.CanConvertFrom(parameterInfo.Type))
            {
                castedParam = converter2.ConvertFromInvariantString(paramStringValue);
                return castedParam;
            }

            try
            {
                castedParam = Convert.ChangeType(paramStringValue, parameterInfo.Type);
                return castedParam;
            }
            catch (Exception ex)
            {
            }

            castedParam = JsonSerializer.Deserialize(paramStringValue, parameterInfo.Type);
            return castedParam;
        }

        public static string GetColor(string textInput, int saturation, int saturationRandomness = 0)
        {
            int seed = textInput.Select(x => (int)x).Sum();
            var random = new Random(seed);

            var remainingColours = new List<int>(3) { 0, 1, 2 };
            int[] colours = new int[3];
            int firstIndex = random.Next(0, 2);
            int randomSaturationToApply = random.Next(0, saturationRandomness);
            colours[remainingColours[firstIndex]] = 255 - randomSaturationToApply;
            remainingColours.RemoveAt(firstIndex);

            int secondIndex = remainingColours[random.Next(0, 1)];
            randomSaturationToApply = random.Next(0, saturationRandomness);
            colours[remainingColours[secondIndex]] = saturation - randomSaturationToApply;
            remainingColours.RemoveAt(secondIndex);

            int flexibleSaturation = random.Next(saturation, 255);
            randomSaturationToApply = random.Next(0, saturationRandomness);
            colours[remainingColours[0]] = flexibleSaturation - randomSaturationToApply;

            StringBuilder stringBuilder = new StringBuilder(6);
            stringBuilder.Append("#");
            stringBuilder.Append(colours[0].ToString("X2"));
            stringBuilder.Append(colours[1].ToString("X2"));
            stringBuilder.Append(colours[2].ToString("X2"));
            string finalColor = stringBuilder.ToString();
            return finalColor;
        }
    }
}