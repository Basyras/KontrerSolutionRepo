//using Basyc.MessageBus.Manager.Application;
//using Basyc.MessageBus.Manager.Application.Initialization;
//using Basyc.MessageBus.Manager.Infrastructure;
//using Basyc.MessageBus.Manager.Infrastructure.MassTransit;
//using Kontrer.OwnerServer.CustomerService.Domain.Customer;
//using Kontrer.OwnerServer.IdGeneratorService.Domain;
//using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
//using Kontrer.Shared.DomainDrivenDesign.Domain;
//using Kontrer.Shared.MessageBus.RequestResponse;
//using MassTransit;

////using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using MudBlazor.Services;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Basyc.MessageBus.Manager.Presentation.Blazor
//{
//    public class Program
//    {
//        public static async Task Main1(string[] args)
//        {
//        }

//        public static async Task Main2(string[] args)
//        {
//            var builder = WebAssemblyHostBuilder.CreateDefault(args);
//            builder.RootComponents.Add<App>("#app");
//            builder.Services.AddMudServices();

//            var assemblies = new Assembly[] { typeof(CreateNewIdCommand).Assembly, typeof(DeleteAccommodationOrderCommand).Assembly, typeof(CreateCustomerCommand).Assembly };

//            builder.Services.AddMessageManager()
//                .AddReqeustClient<BasycMessageBusTypedRequestClient>()
//                .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), assemblies)
//                //.UseInterfaceTypedGenericProvider(typeof(IRequest), typeof(IRequest<>), assemblies)
//                //.UseReqeustClient<MassTransitRequestClient>()
//                //.UseTypedProvider()
//                //.RegisterDomain(x =>
//                //{
//                //    x.DomainName = "CustomerService";
//                //    x.QueryTypes.Add(new Type[] { typeof(DeleteCustomerCommand), typeof(object) });
//                //})
//                //.ChangeFormatting()
//                .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

//            builder.Services.AddMessageBus()
//                .UseProxy()
//                .SetProxyServerUri(new Uri("https://localhost:44371/"));

//            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//            var host = builder.Build();
//            var explorer = host.Services.GetRequiredService<IMessageManager>();
//            explorer.Load();
//            await host.RunAsync();
//        }

//        public static async Task Main3(string[] args)
//        {
//            var assemblies = new Assembly[] { typeof(CreateNewIdCommand).Assembly, typeof(DeleteAccommodationOrderCommand).Assembly, typeof(CreateCustomerCommand).Assembly };
//            var managerBuilder = MessageBusManagerBlazorAppBuilder.Create(args);
//            managerBuilder.services.AddMessageBus()
//                .UseProxy()
//                .SetProxyServerUri(new Uri("https://localhost:44371/"));

//            managerBuilder
//                .AddReqeustClient<BasycMessageBusTypedRequestClient>()
//                .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), assemblies)
//                //.UseInterfaceTypedGenericProvider(typeof(IRequest), typeof(IRequest<>), assemblies)
//                //.UseReqeustClient<MassTransitRequestClient>()
//                //.UseTypedProvider()
//                //.RegisterDomain(x =>
//                //{
//                //    x.DomainName = "CustomerService";
//                //    x.QueryTypes.Add(new Type[] { typeof(DeleteCustomerCommand), typeof(object) });
//                //})
//                //.ChangeFormatting()
//                .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

//            var managerApp = MessageBusManagerBlazorAppBuilder.Build();
//            await managerApp.RunAsync();
//        }

//        //public static async Task Main(string[] args)
//        //{
//        //    var builder = WebAssemblyHostBuilder.CreateDefault(args);
//        //    builder.AddBusManager()
//        //        .AddReqeustClient<BasycMessageBusTypedRequestClient>()
//        //        .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), new System.Reflection.Assembly[] { typeof(DeleteCustomerCommand).Assembly })
//        //        .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();

//        //    builder.Services.AddMessageBus()
//        //        .UseProxy()
//        //        .SetProxyServerUri(new Uri("https://localhost:44371/"));

//        //    var app = builder.Build();
//        //    await app.RunAsync();
//        //}
//    }
//}