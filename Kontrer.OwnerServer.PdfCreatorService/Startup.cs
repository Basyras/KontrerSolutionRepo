using Kontrer.OwnerServer.PdfCreatorService.Services.PdfBuilder;
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
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace Kontrer.OwnerServer.PdfCreatorService
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
              



            services.AddDaprClient(client =>
            {
                client.UseJsonSerializationOptions(options);
            });




            services.AddControllers().AddDapr().AddNewtonsoftJson();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kontrer.OwnerServer.PdfCreatorService", Version = "v1" });
            });


            services.AddPdfBuilder();

            //services.AddMassTransit(transitConfig =>
            //{
            //    transitConfig.UsingRabbitMq((ctx, rabbitConfig)=> 
            //    {

            //        var rabbitUrl = Configuration.GetServiceUri("rabbitmq");
            //        Console.WriteLine("TEEEEEEEEEWST");
            //        Console.WriteLine(rabbitUrl.AbsoluteUri);
            //        rabbitConfig.Host(rabbitUrl);
            //    });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kontrer.OwnerServer.PdfCreatorService v1"));
            }
            else
            {
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCloudEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();
            });
            
        }
    }
}
