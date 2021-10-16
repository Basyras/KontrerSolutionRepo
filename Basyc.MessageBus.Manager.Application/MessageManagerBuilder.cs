using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public class MessageManagerBuilder
    {
        private readonly IServiceCollection services;

        public MessageManagerBuilder(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton<IMessagesExplorerManager, MessagesExplorerManager>();
        }

        public MessageManagerBuilder AddDefaultExplorer<TIQuery, TICommand, TICommandWithResponse>()
        {
            services.AddSingleton<IMessageExplorer>(new DefaultMessageDomainExplorer(typeof(TIQuery), typeof(TICommand), typeof(TICommandWithResponse)));
            return this;
        }

        public MessageManagerBuilder AddDefaultExplorer(Type iQueryType, Type iCommandType, Type iCommandWithResponseType)
        {
            services.AddSingleton<IMessageExplorer>(new DefaultMessageDomainExplorer(iQueryType, iCommandType, iCommandWithResponseType));
            return this;
        }
    }
}