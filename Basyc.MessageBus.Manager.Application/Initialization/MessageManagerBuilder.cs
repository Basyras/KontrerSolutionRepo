using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application.Initialization
{
    public class MessageManagerBuilder
    {
        public readonly IServiceCollection services;

        public MessageManagerBuilder(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton<IMessageManager, MessageManager>();
        }

        public MessageManagerBuilder UseProvider<TMessageExplorer>() where TMessageExplorer : class, IDomainProvider
        {
            services.AddSingleton<IDomainProvider, TMessageExplorer>();
            return this;
        }

        public MessageManagerBuilder UseReqeustClient<TRequestClient>() where TRequestClient : class, IRequestClient
        {
            services.AddSingleton<IRequestClient, TRequestClient>();
            return this;
        }
    }
}