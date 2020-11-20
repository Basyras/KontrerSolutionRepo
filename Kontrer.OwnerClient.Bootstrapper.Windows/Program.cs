using Kontrer.OwnerClient.UI.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Bootstrapper.Windows
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var bootstrapper = new WindowsBootstrapper(args);
            var services = bootstrapper.Create(new ServiceCollection());
            var provider = services.BuildServiceProvider();
            var ui = provider.GetService<IOwnerClientUI>();
            ui.StartClient().GetAwaiter().GetResult(); //Run sync

        }


    }
}
