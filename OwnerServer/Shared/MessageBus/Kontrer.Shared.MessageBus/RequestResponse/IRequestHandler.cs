using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.RequestResponse
{
    public interface IRequestHandler<TRequest> where TRequest : IRequest
    {
        Task Handle(TRequest request, CancellationToken cancellationToken = default);
    }

    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }

    //public interface IRequestWithContextHandler<TRequest, TContext>
    //    where TRequest : IRequestWithContext<TContext>
    //{
    //    Task Handle(TRequest request, TContext context, CancellationToken cancellationToken = default);
    //}

    //public interface IRequestWithContextHandler<TRequest, TResponse, TContext>
    //    where TRequest : IRequestWithContext<TResponse, TContext>
    //    where TResponse : class
    //{
    //    Task<TResponse> Handle(TRequest request, TContext context, CancellationToken cancellationToken = default);
    //}
}