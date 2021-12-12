﻿using Basyc.MessageBus.Manager.Application.Initialization;
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
    public static class MessageManagerBuilderInterfacedExtensions
    {
        public static TypedFormatterBuilder AddInterfacedCQRSProvider(this BusManagerBuilder managerBuilder, Type iQueryType, Type iCommandType, Type iCommandWithResponseType, params Assembly[] assembliesToScan)
        {
            managerBuilder.services.Configure<InterfacedDomainProviderOptions>(options =>
            {
                options.IQueryType = iQueryType;
                options.ICommandType = iCommandType;
                options.ICommandWithResponseType = iCommandWithResponseType;
                options.AssembliesToScan = assembliesToScan;
            });
            managerBuilder.AddProvider<InterfacedDomainProvider>();
            return new TypedFormatterBuilder(managerBuilder.services);
        }

        public static TypedFormatterBuilder AddInterfacedProvider(this BusManagerBuilder managerBuilder, Type iMessageType, Type iMessageWithResponseType, params Assembly[] assemblies)
        {
            managerBuilder.services.Configure<InterfacedDomainProviderOptions>(options =>
            {
                options.IMessageType = iMessageType;
                options.IMessageWithResponseType = iMessageWithResponseType;
                options.AssembliesToScan = assemblies;
            });
            managerBuilder.AddProvider<InterfacedDomainProvider>();
            return new TypedFormatterBuilder(managerBuilder.services);
        }
    }
}