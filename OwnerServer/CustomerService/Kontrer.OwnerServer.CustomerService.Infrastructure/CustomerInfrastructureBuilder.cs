using Basyc.DependencyInjection;
using Basyc.DomainDrivenDesign.DependencyInjection;
using Kontrer.OwnerServer.CustomerService.Application.Interfaces;
using Kontrer.OwnerServer.CustomerService.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class CustomerInfrastructureBuilder : DependencyBuilderBase
    {
        //public readonly IServiceCollection services;
        public CustomerInfrastructureBuilder(IServiceCollection services) : base(services)
        {
            //this.services = services;
        }

        public CustomerInfrastructureBuilder UseRepository<TCustomerRepository>()
            where TCustomerRepository : class, ICustomerRepository
        {
            services.AddScoped<ICustomerRepository, TCustomerRepository>();
            return this;
        }
    }
}