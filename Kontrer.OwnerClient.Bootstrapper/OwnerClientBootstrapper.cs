using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kontrer.OwnerClient.Bootstrapper
{
    public class OwnerClientBootstrapper
    {
        public IServiceCollection BuildApp(IServiceCollection services)
        {
            return services;
        }
    }
}
