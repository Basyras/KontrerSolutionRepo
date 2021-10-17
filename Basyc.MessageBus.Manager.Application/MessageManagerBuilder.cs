using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            services.AddSingleton<IParameterTypeNameFormatter, DefaultParameterTypeNameFormatter>();
            services.AddSingleton<IDomainNameFormatter, DefaultDomainNameFormatter>();
            services.AddSingleton<IRequestNameFormatter, DefaultRequestNameFormatter>();
        }

        public MessageManagerBuilder AddDefaultExplorer<TIQuery, TICommand, TICommandWithResponse>()
        {
            services.Configure<DefaultMessageDomainExplorerOptions>(x =>
            {
                x.IQueryType = typeof(TIQuery);
                x.ICommandType = typeof(TICommand);
                x.ICommandWithResponseType = typeof(TICommandWithResponse);
            });
            services.AddSingleton<IMessageExplorer, DefaultMessageDomainExplorer>();
            return this;
        }

        public MessageManagerBuilder UseDefaultExplorer(Type iQueryType, Type iCommandType, Type iCommandWithResponseType)
        {
            services.Configure<DefaultMessageDomainExplorerOptions>(x =>
            {
                x.IQueryType = iQueryType;
                x.ICommandType = iCommandType;
                x.ICommandWithResponseType = iCommandWithResponseType;
            });
            services.AddSingleton<IMessageExplorer, DefaultMessageDomainExplorer>();
            return this;
        }

        public MessageManagerBuilder UseDomainNameFormatter<TDomainNameFormatter>()
            where TDomainNameFormatter : class, IDomainNameFormatter
        {
            services.RemoveAll<IDomainNameFormatter>();
            services.AddSingleton<IDomainNameFormatter, TDomainNameFormatter>();
            return this;
        }
    }
}