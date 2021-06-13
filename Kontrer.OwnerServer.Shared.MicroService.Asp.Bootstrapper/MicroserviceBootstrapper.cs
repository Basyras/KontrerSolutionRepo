﻿using Kontrer.OwnerServer.Shared.Asp;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public static class MicroserviceBootstrapper
    {
        public static IHostBuilder CreateMicroserviceHostBuilder<TStartup, TConsumer>(string[] args) where TStartup : class, IStartupClass
        {
            return Host.CreateDefaultBuilder().ConfigureMicroservice<TStartup>(typeof(TConsumer).Assembly);
        }

        public static IHostBuilder CreateMicroserviceHostBuilder<TStartup>(string[] args) where TStartup : class, IStartupClass
        {
            return Host.CreateDefaultBuilder().ConfigureMicroservice<TStartup>(typeof(TStartup).Assembly);
        }
    }
}