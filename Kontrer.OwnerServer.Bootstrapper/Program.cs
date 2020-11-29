using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Kontrer.OwnerServer.Bootstrapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AspApiBootstrapper bootstrapper = new AspApiBootstrapper();
            var host  = bootstrapper.CreateHostBuilder(args).Build();
            host.Run();
        }

      


    }
}
