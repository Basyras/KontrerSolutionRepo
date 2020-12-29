using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Dapr.MessageBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Dapr
{
    public class CommonMicroserviceStartup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public CommonMicroserviceStartup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;            
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDaprClient();
            services.AddSingleton(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });

            services.AddControllers()
            .AddDapr();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = _environment.ApplicationName, Version = "v1" });
            });

            services.AddSingleton<IMessageBusManager, DaprMessageBusManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app/*, IWebHostEnvironment env*/)
        {   
            var sss = app.ApplicationServices.GetRequiredService<IHostEnvironment>();          

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", _environment.ApplicationName + " v1"));
            }


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCloudEvents();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();

                var bus = endpoints.ServiceProvider.GetRequiredService<IMessageBusManager>();
                foreach (var subs in bus.BusSubscriptions)
                {
                    //var subEndpoint = subs.RequestType.Name;
                    endpoints.MapPost(subs.Topic, (Microsoft.AspNetCore.Http.RequestDelegate)subs.Handler).WithTopic(bus.BusName, subs.Topic);
                }

            });


        }
    }
}
