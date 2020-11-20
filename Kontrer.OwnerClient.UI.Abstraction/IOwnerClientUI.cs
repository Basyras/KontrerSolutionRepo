using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.UI.Abstraction
{
    public interface IOwnerClientUI
    {
        Task StartClient();
        Task StartSplash(CancellationToken appCancelToken, string startingmessage = null, int? startingPercentage = null);
        void CloseSplash();
    }


}
