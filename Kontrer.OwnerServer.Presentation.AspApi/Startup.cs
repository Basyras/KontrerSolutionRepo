using Kontrer.OwnerServer.Presentation.AspApi.Bootstrapping;
using Kontrer.OwnerServer.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Presentation.AspApi
{
    
    public class Startup
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {



            services.AddControllers();//.AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kontrer.OwnerServer.Presentation.AspApi", Version = "v1" });
            });

            services.AddDaprClient(client =>
            {                
                client.UseJsonSerializationOptions(options);
            });

            //ApiBootstrapper.ConfigureServices(services);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kontrer.OwnerServer.Presentation.AspApi v1"));
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCloudEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSubscribeHandler();
                
                //endpoints.MapPost("/TestStringChanged", async context =>
                //{
                //    //var confirmation = await JsonSerializer.DeserializeAsync<OrderConfirmation>(context.Request.Body, new JsonSerializerOptions()
                //    //{
                //    //    PropertyNameCaseInsensitive = true
                //    //});
                //    //broker.Complete(confirmation);

                //    var newString = await JsonSerializer.DeserializeAsync<string>(context.Request.Body, options);

                //    var logger = endpoints.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                //    logger.LogDebug($"TestStringChanged detected, new value {newString}");


                //}).WithTopic(Constants.MessageBusName, "TestStringChanged");
            });
        }
    }
}
