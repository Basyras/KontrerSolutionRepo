using Basyc.MessageBus.Manager.Application;
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
        public IMessagesExplorerManager Explorer { get; private set; }

        [Inject]
        public IMessageBusManager MessageBusManager { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; }

        public List<DomainInfoViewModel> DomainVMs { get; set; } = new List<DomainInfoViewModel>();

        protected override void OnInitialized()
        {
            DomainVMs = Explorer.Domains.Select(x => new DomainInfoViewModel(x, x.Messages.Select(x => new RequestInfoViewModel(x)).OrderBy(x=>x.RequestInfo.IsCommand).ToList())).ToList();

            base.OnInitialized();
        }

        public async Task SendMessage(RequestItem requestItem)
        {
            var message = requestItem.Request;
            object[] castedParameters = new object[message.Parameters.Count];
            for (int i = 0; i < message.Parameters.Count; i++)
            {
                var paramInfo = message.Parameters[i];
                var paramStringValue = requestItem.ParameterValues[i];
                if (paramStringValue == "@null")
                {
                    castedParameters[i] = null;
                }
                else
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(paramInfo);
                    object castedParam;
                    if (converter.CanConvertFrom(typeof(string)))
                    {
                        castedParam = converter.ConvertFromInvariantString(paramStringValue);
                    }
                    else
                    {
                        castedParam = JsonSerializer.Deserialize(paramStringValue, paramInfo.Type);
                    }
                    castedParameters[i] = castedParam;
                }
            }

            var messageInstance = Activator.CreateInstance(message.Type, castedParameters);
            if (message.HasResponse)
            {
                var response = await MessageBusManager.RequestAsync(message.Type, messageInstance, message.ResponseType);
                await DialogService.ShowMessageBox(null, JsonSerializer.Serialize(response, message.ResponseType), "ok", null, null, new DialogOptions() { CloseButton = false, NoHeader = true });
            }
            else
            {
                await MessageBusManager.SendAsync(message.Type, messageInstance);
            }
        }

        private static object ParseParamInputValue(string paramStringValue, RequestParameterInfo parameterInfo)
        {
            if (paramStringValue == "@null")
            {
                return null;
            }

            if(paramStringValue == String.Empty)
            {
                return parameterInfo.Type.GetDefaultValue();
            }

            TypeConverter converter = TypeDescriptor.GetConverter(parameterInfo);
            object castedParam;
            if (converter.CanConvertFrom(typeof(string)))
            {
                castedParam = converter.ConvertFromInvariantString(paramStringValue);
            }
            else
            {
                castedParam = JsonSerializer.Deserialize(paramStringValue, parameterInfo.Type);
            }
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