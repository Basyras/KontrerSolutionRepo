using Basyc.Microservice.DomainDrivenDesign;
using Kontrer.OwnerServer.CustomerService.Application;
using Kontrer.OwnerServer.CustomerService.Application.Customer;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.CustomerService.Infrastructure;
using Basyc.DomainDrivenDesign.Domain;
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
using Basyc.MicroService.Asp.Bootstrapper;

namespace Kontrer.OwnerServer.CustomerService.Presentation.AspApi
{
    public class Program
    {
        private const string debugConnectionString = "Server=(localdb)\\mssqllocaldb;Database=CustomerServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static async Task Main(string[] args)
        {
            var builder = MicroserviceBootstrapper.CreateBuilder<Startup>(args);

            builder.AddMessageBus()
                 .RegisterBasycRequestHandlers<CustomerServiceApplicationAssemblyMarker>()
                 .AddMassTransitProvider();

            new CustomerInfrastructureBuilder(builder.services)
                .AddEFRespository()
                .AddSqlServer(debugConnectionString);

            await builder.Back()
                 .Build()
                 .RunAsync();
        }
    }
}