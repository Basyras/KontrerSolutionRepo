using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public interface IRequestClient
    {
        public Task<RequestResult> TrySendRequest(Request request);
    }
}