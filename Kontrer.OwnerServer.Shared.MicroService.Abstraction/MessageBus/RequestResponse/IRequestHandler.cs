using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse
{
    public interface IRequestHandler<TRequest> where TRequest : IRequest
    {
        Task Handle(CancellationToken cancellationToken = default);
    }

    public interface IRequestHandler<TResponse, TRequest>
        where TRequest : IRequest<TResponse>
        where TResponse : class

    {
        Task<TResponse> Handle(CancellationToken cancellationToken = default);
    }
}