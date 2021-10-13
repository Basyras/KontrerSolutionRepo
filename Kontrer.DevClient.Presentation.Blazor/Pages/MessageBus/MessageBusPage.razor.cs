using Kontrer.Shared.DomainDrivenDesign.Domain;
using Kontrer.Shared.MessageBus;
using Kontrer.Shared.MessageBus.RequestResponse;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kontrer.DevClient.Presentation.Blazor.Pages.MessageBus
{
    public partial class MessageBusPage
    {
        [Inject]
        public IMessageBusManager MessageBusManager { get; set; }

        public List<DomainViewModel> Domains { get; set; } = new List<DomainViewModel>();

        public MessageBusPage()
        {
        }

        protected override void OnInitialized()
        {
            var reqs = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(x => x.IsAssignableTo(typeof(IRequest))));
            var mess = reqs.Select(x => new MessageViewModel(x, x.GetConstructors().First().GetParameters().Select(x => x.ParameterType).ToList())).ToList();
            var reqs2 = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(x => x.IsAssignableTo(typeof(IRequest<>))));
            mess.AddRange(reqs2.Select(x => new MessageViewModel(x, x.GetConstructors().First().GetParameters().Select(x => x.ParameterType).ToList())).ToList());
            Domains.Add(new DomainViewModel("all", mess));

            base.OnInitialized();
        }

        public void SendMessage(MessageViewModel message)
        {
        }
    }
}