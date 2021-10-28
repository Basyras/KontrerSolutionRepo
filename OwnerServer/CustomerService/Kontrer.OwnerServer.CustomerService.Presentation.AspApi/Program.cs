using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Presentation.Blazor;
using Basyc.Microservice.DomainDrivenDesign;
using Kontrer.OwnerServer.CustomerService.Application;
using Kontrer.OwnerServer.CustomerService.Application.Customer;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.CustomerService.Infrastructure;
using Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Presentation.AspApi
{
    public class Program
    {
        private const string debugConnectionString = "Server=(localdb)\\mssqllocaldb;Database=CustomerServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static async Task Main(string[] args)
        {
            //MicroserviceBootstrapper.CreateMicroserviceHostBuilder<Startup, GetCustomersQueryHandler>(args)
            //    .Build()
            //    .Run();
            var tasks = new List<Task>();

            var builder = MicroserviceBootstrapper
                 .CreateBuilder<Startup, GetCustomersQueryHandler>(args);

            builder.AddMessageBus(typeof(CustomerServiceApplicationAssemblyMarker).Assembly);

            new CustomerInfrastructureBuilder(builder.services)
                .UseEFRespository()
                .UseSqlServer(debugConnectionString);

            //tasks.Add(builder.Back()
            //     .Build()
            //     .RunAsync());

            var assemblies = new Assembly[] { typeof(CreateCustomerCommand).Assembly };
            var managerBuilder = MessageBusManagerBlazorAppBuilder.Create(args);
            managerBuilder.services.AddMessageBus()
                .UseProxy()
                .SetProxyServerUri(new Uri("https://localhost:44371/"));

            managerBuilder
                .UseReqeustClient<BasycMessageBusTypedRequestClient>()
                .UseInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), assemblies)
                .UseDomainNameFormatter<TypedDDDDomainNameFormatter>();

            var managerApp = MessageBusManagerBlazorAppBuilder.Build();
            tasks.Add(managerApp.RunAsync());

            Task.WaitAll(tasks.ToArray());
        }
    }
}