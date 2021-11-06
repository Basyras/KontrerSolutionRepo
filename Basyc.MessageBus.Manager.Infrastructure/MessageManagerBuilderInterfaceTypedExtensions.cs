using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
    public static class MessageManagerBuilderInterfaceTypedExtensions
    {
        //public static TypedFormatterBuilder AddInterfaceTypedCQRSProvider<TIQuery, TICommand, TICommandWithResponse>(this BusManagerBuilder managerBuilder, Assembly[] assemblies)
        //{
        //    managerBuilder.services.Configure<InterfaceTypedDomainProviderOptions>(options =>
        //    {
        //        options.IQueryType = typeof(TIQuery);
        //        options.ICommandType = typeof(TICommand);
        //        options.ICommandWithResponseType = typeof(TICommandWithResponse);
        //        options.AssembliesToScan = assemblies.ToList();
        //    });
        //    managerBuilder.services.AddSingleton<IDomainInfoProvider, InterfaceTypedDomainProvider>();
        //    return new TypedFormatterBuilder(managerBuilder.services);
        //}

        public static TypedFormatterBuilder AddInterfaceTypedCQRSProvider(this BusManagerBuilder managerBuilder, Type iQueryType, Type iCommandType, Type iCommandWithResponseType, params Assembly[] assemblies)
        {
            managerBuilder.services.Configure<InterfaceTypedDomainProviderOptions>(options =>
            {
                options.IQueryType = iQueryType;
                options.ICommandType = iCommandType;
                options.ICommandWithResponseType = iCommandWithResponseType;
                options.AssembliesToScan = assemblies.ToList();
            });
            managerBuilder.AddProvider<InterfaceTypedDomainProvider>();
            return new TypedFormatterBuilder(managerBuilder.services);
        }

        //public static TypedFormatterBuilder AddInterfaceTypedCQRSProvider<TQuery, TCommand, TCommandWithResponse>(this BusManagerBuilder managerBuilder, params Assembly[] assemblies)
        //{
        //    return AddInterfaceTypedCQRSProvider(managerBuilder, typeof(TQuery), typeof(TQuery), typeof(TQuery), assemblies);
        //}

        public static TypedFormatterBuilder AddInterfaceTypedProvider(this BusManagerBuilder managerBuilder, Type iMessageType, Type iMessageWithResponseType, params Assembly[] assemblies)
        {
            managerBuilder.services.Configure<InterfaceTypedDomainProviderOptions>(options =>
            {
                options.IMessageType = iMessageType;
                options.IMessageWithResponseType = iMessageWithResponseType;
                options.AssembliesToScan = assemblies.ToList();
            });
            managerBuilder.AddProvider<InterfaceTypedDomainProvider>();
            return new TypedFormatterBuilder(managerBuilder.services);
        }
    }
}