using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse
{
    public interface IRequest<TResponse>
        where TResponse : class
    {
    }

    public interface IRequest
    {
    }
}