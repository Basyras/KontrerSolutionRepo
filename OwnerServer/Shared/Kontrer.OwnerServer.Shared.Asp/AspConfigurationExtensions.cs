using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.Asp
{
    public static class AspConfigurationExtensions
    {
        /// <summary>
        /// Register generic services for all ASP project (e.g. swagger)
        /// </summary>
        public static IWebHostBuilder ConfigureAsp<TStartup>(this IWebHostBuilder webBuilder, string assemblyName)
            where TStartup : class, IStartupClass
        {            
            webBuilder.UseStartupWorkaround<TStartup>(assemblyName);
            webBuilder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IStartupFilter, AspDefaultStartupFilter>();
                services.AddControllers().FixJsonSerialization(); //Required for swagger
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = context.HostingEnvironment.ApplicationName, Version = "v1" });
                });
                services.AddHealthChecks();
                //services.AddCors(options =>
                //{
                //    options.
                //});
            });

            return webBuilder;
        }

        /// <summary>
        /// Workaround to setup Startup class from different assembly. Bug described here: https://github.com/dotnet/aspnetcore/issues/7315
        /// </summary>
        public static IWebHostBuilder UseStartupWorkaround<TStartup>(this IWebHostBuilder webBuilder, string assemblyName = null)
            where TStartup : class, IStartupClass
        {
            webBuilder.UseSetting(WebHostDefaults.ApplicationKey, assemblyName);
            webBuilder.UseStartup<TStartup>();
            return webBuilder;
        }

        /// <summary>
        /// Using the old school NewtonsoftJson istead of new microsoft seriliazer
        /// </summary>
        public static IMvcBuilder FixJsonSerialization(this IMvcBuilder mvcBuilder)
        {
            //mvcBuilder.AddJsonOptions()
            //var jsonSeri  =services.GetRequiredService<>
            //JsonConvert.DefaultSettings =
            mvcBuilder.AddNewtonsoftJson();
            return mvcBuilder;
        }
    }
}