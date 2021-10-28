using Kontrer.OwnerServer.CustomerService.Application.Interfaces;
using Kontrer.Shared.Repositories.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Infrastructure.EntityFramework
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

        public EFRepositoryBuilder UseSqlServer(string connectionString)
        {
            services.AddDbContext<DbContext, CustomerServiceDbContext>(options =>
                options.UseSqlServer(connectionString));
            return this;
        }
    }
}