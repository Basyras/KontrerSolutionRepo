using Dapr.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Actors.PdfCreator
{
    public interface ITestActor : IActor
    {
        Task<string> TestMethod();
    }
}
