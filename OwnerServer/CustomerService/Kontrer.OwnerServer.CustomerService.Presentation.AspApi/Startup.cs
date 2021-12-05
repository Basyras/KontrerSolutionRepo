using Basyc.Asp;
using Kontrer.OwnerServer.CustomerService.Application.Interfaces;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.CustomerService.Infrastructure.EntityFramework;
using Basyc.DomainDrivenDesign.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Presentation.AspApi
{
    public class Startup : IStartupClass
    {
        private const string debugConnectionString = "Server=(localdb)\\mssqllocaldb;Database=CustomerServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<DbContext, CustomerServiceDbContext>(options =>
            //         options.UseSqlServer(debugConnectionString));
            //services.AddScoped<ICustomerRepository, EFCustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<DbContext>();
            //    db.Database.Migrate();
            //}

            //try
            //{
            //    var assembliesToScan = new Assembly[] { typeof(CreateCustomerCommand).Assembly };
            //    var managerBuilder = MessageBusManagerBlazorAppBuilder.Create(new string[0]);
            //    managerBuilder.services.AddMessageBus()
            //        .UseProxy()
            //        .SetProxyServerUri(new Uri("https://localhost:44371/"));

            //    managerBuilder
            //        .AddReqeustClient<BasycMessageBusTypedRequestClient>()
            //        .AddInterfaceTypedCQRSProvider(typeof(IQuery<>), typeof(ICommand), typeof(ICommand<>), assembliesToScan)
            //        .AddDomainNameFormatter<TypedDDDDomainNameFormatter>();
            //    var managerApp = MessageBusManagerBlazorAppBuilder.Build();
            //    managerApp.RunAsync().GetAwaiter().GetResult();
            //}
            //catch (Exception ex)
            //{
            //}

            //app.MapWhen(ctx => ctx.Request.Path, second =>
            //{
            //    second.Use((ctx, nxt) =>
            //    {
            //        ctx.Request.Path = "/SecondApp" + ctx.Request.Path;
            //        return nxt();
            //    });

            //    second.UseBlazorFrameworkFiles("/SecondApp");
            //    second.UseStaticFiles();
            //    second.UseStaticFiles("/SecondApp");
            //    second.UseRouting();

            //    second.UseEndpoints(endpoints =>
            //    {
            //        endpoints.MapControllers();
            //        endpoints.MapFallbackToFile("/SecondApp/{*path:nonfile}",
            //            "SecondApp/index.html");
            //    });
            //});

            //app.Map(new Microsoft.AspNetCore.Http.PathString("/SecondApp"), second =>
            //{
            //    second.Use((ctx, nxt) =>
            //    {
            //        ctx.Request.Path = "/SecondApp" + ctx.Request.Path;
            //        return nxt();
            //    });

            //    second.UseBlazorFrameworkFiles("/SecondApp");
            //    second.UseStaticFiles();
            //    second.UseStaticFiles("/SecondApp");
            //    second.UseRouting();

            //    second.UseEndpoints(endpoints =>
            //    {
            //        endpoints.MapControllers();
            //        endpoints.MapFallbackToFile("/SecondApp/{*path:nonfile}",
            //            "SecondApp/index.html");
            //    });
            //});
        }
    }
}