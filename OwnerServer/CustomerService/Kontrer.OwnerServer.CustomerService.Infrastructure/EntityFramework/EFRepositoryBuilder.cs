using Basyc.Repositories.EF;
using Kontrer.OwnerServer.CustomerService.Application.Interfaces;
using Kontrer.OwnerServer.CustomerService.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class EFRepositoryBuilder
    {
        public readonly IServiceCollection services;

        public EFRepositoryBuilder(IServiceCollection services)
        {
            this.services = services;
            services.AddTransient<IStartupFilter, EFMigrationStartupFilter<CustomerServiceDbContext>>();
            services.AddScoped<ICustomerRepository, EFCustomerRepository>();
        }

        public EFRepositoryBuilder AddSqlServer(string connectionString)
        {
            services.AddDbContext<CustomerServiceDbContext>(options =>
                options.UseSqlServer(connectionString));
            return this;
        }
    }
}