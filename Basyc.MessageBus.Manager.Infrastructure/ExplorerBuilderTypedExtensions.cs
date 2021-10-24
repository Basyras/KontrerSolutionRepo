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
    public static class ExplorerBuilderTypedExtensions
    {
        public static TypedExplorerBuilder UseTypedCQRSProvider<TIQuery, TICommand, TICommandWithResponse>(this MessageManagerBuilder managerBuilder, Assembly[] assemblies)
        {
            managerBuilder.services.Configure<TypedDomainProviderOptions>(options =>
            {
                options.IQueryType = typeof(TIQuery);
                options.ICommandType = typeof(TICommand);
                options.ICommandWithResponseType = typeof(TICommandWithResponse);
                options.AssembliesToScan = assemblies.ToList();
            });
            managerBuilder.services.AddSingleton<IDomainProvider, TypedDomainProvider>();
            return new TypedExplorerBuilder(managerBuilder.services);
        }

        public static TypedExplorerBuilder UseTypedCQRSProvider(this MessageManagerBuilder managerBuilder, Type iQueryType, Type iCommandType, Type iCommandWithResponseType, Assembly[] assemblies)
        {
            managerBuilder.services.Configure<TypedDomainProviderOptions>(options =>
            {
                options.IQueryType = iQueryType;
                options.ICommandType = iCommandType;
                options.ICommandWithResponseType = iCommandWithResponseType;
                options.AssembliesToScan = assemblies.ToList();
            });
            managerBuilder.UseProvider<TypedDomainProvider>();
            return new TypedExplorerBuilder(managerBuilder.services);
        }

        public static TypedExplorerBuilder UseTypedGenericProvider(this MessageManagerBuilder managerBuilder, Type iMessageType, Type iMessageWithResponseType, Assembly[] assemblies)
        {
            managerBuilder.services.Configure<TypedDomainProviderOptions>(options =>
            {
                options.IMessageType = iMessageType;
                options.IMessageWithResponseType = iMessageWithResponseType;
                options.AssembliesToScan = assemblies.ToList();
            });
            managerBuilder.UseProvider<TypedDomainProvider>();
            return new TypedExplorerBuilder(managerBuilder.services);
        }
    }
}