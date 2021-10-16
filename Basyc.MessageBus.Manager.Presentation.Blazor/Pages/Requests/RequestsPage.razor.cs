using Basyc.MessageBus.Manager.Application;
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
            DomainVMs = Explorer.Domains.Select(x => new DomainInfoViewModel(x, x.Messages.Select(x => new RequestInfoViewModel(x)).ToList())).ToList();

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

        private void SetParamValue(RequestInfoViewModel message, int index, string value)
        {
            message.ParameterValues[index] = value;
        }

        public static string GetColor2(string raw)
        {
            //byte[] data = Encoding.UTF8.GetBytes(raw.GetHashCode().ToString());
            //var hashNumber = BitConverter.ToInt32(data);
            //var hashNumber = Convert.ToInt32((255 / (Math.Pow(0.2, -0.002 * hashNumber))));
            var seed = raw.Select(x => (int)x).Sum();
            var random = new Random(seed);
            int saturation = 20;
            int red = 255 - random.Next(0, saturation);
            int green = 255 - random.Next(0, saturation);
            int blue = 255 - random.Next(0, saturation);

            string finalColor = "#" + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
            return finalColor;
            //return "#" + BitConverter.ToString(data).Replace("-", string.Empty).Substring(0, 6);
        }

        public static string GetColor3(string raw)
        {
            int seed = raw.Select(x => (int)x).Sum();
            var random = new Random(seed);
            int saturation = 20;
            int remainingHuePoints = saturation;
            int pointsToApply = random.Next(0, saturation);
            remainingHuePoints -= pointsToApply;
            int red = 255 - pointsToApply;
            pointsToApply = random.Next(0, saturation);
            remainingHuePoints -= pointsToApply;
            int green = 255 - pointsToApply;
            int blue = 255 - remainingHuePoints;

            string finalColor = "#" + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
            return finalColor;
        }

        public static string GetColor4(string raw, int saturation)
        {
            int seed = raw.Select(x => (int)x).Sum();
            var random = new Random(seed);
            int remainingSaturation = 255 - saturation;

            int pointsToApply = random.Next(0, remainingSaturation);
            remainingSaturation -= pointsToApply;

            int red = 255 - pointsToApply;

            pointsToApply = random.Next(0, remainingSaturation);
            remainingSaturation -= pointsToApply;

            int green = 255 - pointsToApply;

            int blue = 255 - remainingSaturation;

            string finalColor = "#" + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
            return finalColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textInput"></param>
        /// <param name="saturation">0-255</param>
        /// <param name="saturationRandomness">0-255</param>
        /// <returns></returns>
        public static string GetColor5(string textInput, int saturation, int saturationRandomness = 0)
        {
            var randomChar = textInput[textInput.Length / 3];
            //int seed = textInput.Select(x => (int)x).Sum();
            int seed = (int)randomChar;
            var random = new Random(seed);
            int remainingHuePoints = 255 - saturation;

            int saturationToApply = random.Next(0, remainingHuePoints);
            remainingHuePoints -= saturationToApply;
            int randomSaturationToApply = random.Next(0, saturationRandomness);
            int red = 255 - (saturationToApply + randomSaturationToApply);

            saturationToApply = random.Next(0, remainingHuePoints);
            remainingHuePoints -= saturationToApply;
            randomSaturationToApply = random.Next(0, saturationRandomness);
            int green = 255 - (saturationToApply + randomSaturationToApply);

            randomSaturationToApply = random.Next(0, saturationRandomness);
            int blue = 255 - (remainingHuePoints + randomSaturationToApply);

            StringBuilder stringBuilder = new StringBuilder(6);
            stringBuilder.Append("#");
            stringBuilder.Append(red.ToString("X2"));
            stringBuilder.Append(green.ToString("X2"));
            stringBuilder.Append(blue.ToString("X2"));
            string finalColor = stringBuilder.ToString();
            return finalColor;
        }

        public static string GetColor(string textInput, int saturation, int saturationRandomness = 0)
        {
            int seed = textInput[(int)Math.Ceiling(textInput.Length / (double)3)] + textInput.Length;
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