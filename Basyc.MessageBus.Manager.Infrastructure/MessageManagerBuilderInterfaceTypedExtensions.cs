using Basyc.MessageBus.Manager.Application.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Infrastructure
{
    public static class MessageManagerBuilderInterfaceTypedExtensions
    {
        public static TypedFormatterBuilder UseInterfaceTypedCQRSProvider<TIQuery, TICommand, TICommandWithResponse>(this MessageManagerBuilder managerBuilder, Assembly[] assemblies)
        {
            managerBuilder.services.Configure<InterfaceTypedDomainProviderOptions>(options =>
            {
                options.IQueryType = typeof(TIQuery);
                options.ICommandType = typeof(TICommand);
                options.ICommandWithResponseType = typeof(TICommandWithResponse);
                options.AssembliesToScan = assemblies.ToList();
            });
            managerBuilder.services.AddSingleton<IDomainInfoProvider, InterfaceTypedDomainProvider>();
            return new TypedFormatterBuilder(managerBuilder.services);
        }

        public static TypedFormatterBuilder UseInterfaceTypedCQRSProvider(this MessageManagerBuilder managerBuilder, Type iQueryType, Type iCommandType, Type iCommandWithResponseType, Assembly[] assemblies)
        {
            managerBuilder.services.Configure<InterfaceTypedDomainProviderOptions>(options =>
            {
                options.IQueryType = iQueryType;
                options.ICommandType = iCommandType;
                options.ICommandWithResponseType = iCommandWithResponseType;
                options.AssembliesToScan = assemblies.ToList();
            });
            managerBuilder.UseProvider<InterfaceTypedDomainProvider>();
            return new TypedFormatterBuilder(managerBuilder.services);
        }

        public static TypedFormatterBuilder UseInterfaceTypedGenericProvider(this MessageManagerBuilder managerBuilder, Type iMessageType, Type iMessageWithResponseType, Assembly[] assemblies)
        {
            managerBuilder.services.Configure<InterfaceTypedDomainProviderOptions>(options =>
            {
                options.IMessageType = iMessageType;
                options.IMessageWithResponseType = iMessageWithResponseType;
                options.AssembliesToScan = assemblies.ToList();
            });
            managerBuilder.UseProvider<InterfaceTypedDomainProvider>();
            return new TypedFormatterBuilder(managerBuilder.services);
        }
    }
}