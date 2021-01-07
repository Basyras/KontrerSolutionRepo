using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public class DefaultStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
         
            return (IApplicationBuilder app) =>
            {
                var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", env.ApplicationName + " v1"));
                }

                app.UseHttpsRedirection();
                app.UseRouting();

                app.UseAuthorization();
                next(app);
            };

        }

     
    }
}