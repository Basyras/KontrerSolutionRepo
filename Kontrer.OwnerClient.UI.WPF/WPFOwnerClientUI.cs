using Kontrer.OwnerClient.UI.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.UI.WPF
{
    public class WPFOwnerClientUI : IOwnerClientUI
    {
        public void CloseSplash()
        {
            //throw new NotImplementedException();
        }

     
        public Task StartClient()
        {
            //throw new NotImplementedException();
            var app = new App();
            var mainWindow = new MainWindow();
            app.Run(mainWindow);
            return Task.CompletedTask;
        }

        public Task StartSplash(CancellationToken appCancelToken, string startingmessage = null, int? startingPercentage = null)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
