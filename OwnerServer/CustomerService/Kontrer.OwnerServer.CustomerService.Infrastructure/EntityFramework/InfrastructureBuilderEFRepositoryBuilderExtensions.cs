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
    public static class InfrastructureBuilderEFRepositoryBuilderExtensions
    {
        public static EFRepositoryBuilder AddEFRespository(this CustomerInfrastructureBuilder infrastructureBuilder)
        {
            return new EFRepositoryBuilder(infrastructureBuilder.services);
        }
    }
}