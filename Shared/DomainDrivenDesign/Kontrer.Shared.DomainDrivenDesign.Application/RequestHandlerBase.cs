using Kontrer.Shared.DomainDrivenDesign.Domain;
using Kontrer.Shared.MessageBus.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.DomainDrivenDesign.Application
{
    //public abstract class RequestHandlerBase<TRequest> : IRequestHandler<TRequest>
    //    where TRequest : class, IRequest
    //{
    //    public abstract Task Handle(TRequest command, CancellationToken cancellationToken = default);
    //}

    //public abstract class RequestHandlerBase<TRequest, TReponse> : IRequestHandler<TRequest, TReponse>
    //    where TReponse : class
    //    where TRequest : class, IRequest<TReponse>
    //{
    //    public abstract Task<TReponse> Handle(TRequest command, CancellationToken cancellationToken = default);
    //}
}