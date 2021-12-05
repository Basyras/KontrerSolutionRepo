using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.MassTransit;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager;

public static class MassTransitMessageManagerBuilderExtensions
{
    public static TypedProviderBuilder UseMasstransit(this BusManagerBuilder managerBuilder)
    {
        UseMasstransitReqeustClient(managerBuilder);
        var typedBuilder = managerBuilder.UseTypedProvider();
        return typedBuilder;
    }

    public static BusManagerBuilder UseMasstransitReqeustClient(this BusManagerBuilder managerBuilder)
    {
        managerBuilder.services.AddSingleton<IBusClient, MassTransitBusClient>();
        return managerBuilder;
    }
}
