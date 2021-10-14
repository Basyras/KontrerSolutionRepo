using Kontrer.OwnerServer.IdGeneratorService.Domain;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using Kontrer.Shared.Helpers;
using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kontrer.DevClient.Presentation.Blazor.Pages.MessageBus
{
    public partial class MessageBusPage
    {
        [Inject]
        public IMessageBusManager MessageBusManager { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; }

        public List<DomainViewModel> Domains { get; set; } = new List<DomainViewModel>();

        public MessageBusPage()
        {
        }

        protected override void OnInitialized()
        {
            LoadDomains();
            base.OnInitialized();
        }

        public async Task SendMessage(MessageViewModel message)
        {
            object[] castedParameters = new object[message.Parameters.Count];
            for (int i = 0; i < message.Parameters.Count; i++)
            {
                var paramType = message.Parameters[i];
                var paramStringValue = message.ParametersValues[i];
                TypeConverter converter = TypeDescriptor.GetConverter(paramType);
                object castedParam;
                if (converter.CanConvertFrom(typeof(string)))
                {
                    castedParam = converter.ConvertFromInvariantString(paramStringValue);
                }
                else
                {
                    castedParam = JsonSerializer.Deserialize(paramStringValue, paramType);
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

        private void LoadDomains()
        {
            var iRequestTType = typeof(IRequest<>);
            var iRequestType = typeof(IRequest);
            var allAssemblies = GetAssembliesWithCommands();
            var messages = new List<MessageViewModel>();

            foreach (var type in allAssemblies.SelectMany(assembly => assembly.GetTypes()))
            {
                if (type.IsClass is false)
                    continue;
                if (type.IsAbstract is true)
                    continue;

                if (type.GetInterface(iRequestTType.Name) is not null)
                {
                    var responseType = GenericsHelper.GetGenericArgumentsFromParent(type, typeof(IRequest<>))[0];
                    var paramTypes = type.GetConstructors().First().GetParameters().Select(x => x.ParameterType).ToList();
                    messages.Add(new MessageViewModel(type, paramTypes, true, responseType));
                    continue;
                }

                if (type.GetInterface(iRequestType.Name) is not null)
                {
                    var paramTypes = type.GetConstructors().First().GetParameters().Select(x => x.ParameterType).ToList();
                    messages.Add(new MessageViewModel(type, paramTypes, false));
                    continue;
                }
            }

            Domains.Add(new DomainViewModel("All", messages));
        }

        //public static List<Assembly> GetAllPresentAssemblies()
        //{
        //    var returnAssemblies = new List<Assembly>();
        //    var loadedAssemblies = new HashSet<string>();
        //    var assembliesToCheck = new Queue<Assembly>();
        //    var entryAssembly = typeof(Program).Assembly;
        //    assembliesToCheck.Enqueue(entryAssembly);

        //    while (assembliesToCheck.Any())
        //    {
        //        var assemblyToCheck = assembliesToCheck.Dequeue();
        //        var referencedAssemblies = assemblyToCheck.GetReferencedAssemblies();
        //        foreach (var referencedAssembly in referencedAssemblies)
        //        {
        //            if (!loadedAssemblies.Contains(referencedAssembly.FullName))
        //            {
        //                var loadedAssembly = Assembly.Load(referencedAssembly);
        //                assembliesToCheck.Enqueue(loadedAssembly);
        //                loadedAssemblies.Add(referencedAssembly.FullName);
        //                returnAssemblies.Add(loadedAssembly);
        //            }
        //        }
        //    }

        //    return returnAssemblies;
        //}

        //private static List<Assembly> GetAllAssemblies()
        //{
        //    var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        //    var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

        //    var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
        //    var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

        //    toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
        //    //toLoad.ForEach(x =>)

        //    return loadedAssemblies;
        //}

        //private static List<Assembly> LaodReferencedAssemblies(string assemblyPathToLoad)
        //{
        //    var assemblyName = AssemblyName.GetAssemblyName(assemblyPathToLoad);
        //    var loadedAssembly = AppDomain.CurrentDomain.Load(assemblyName);

        //}

        private static Assembly[] GetAssembliesWithCommands()
        {
            var types = new Type[] { typeof(CreateNewIdCommand), typeof(CreateAccommodationOrderCommand) };
            return types.Select(x => x.Assembly).ToArray();
        }

        private void SetParamValue(List<string> parameters, int index, string value)
        {
            if (parameters.Count > index)
            {
                parameters[index] = value;
            }
        }
    }
}