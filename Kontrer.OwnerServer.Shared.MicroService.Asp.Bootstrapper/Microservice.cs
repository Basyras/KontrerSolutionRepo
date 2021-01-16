﻿using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper
{
    public static class Microservice
    {
        public static IHostBuilder CreateMicroservice<TStartup>(string[] args) where TStartup : class,IStartupClass
        {          
           return  Host.CreateDefaultBuilder().ConfigureMicroservice<TStartup>();
        }
    }
}
