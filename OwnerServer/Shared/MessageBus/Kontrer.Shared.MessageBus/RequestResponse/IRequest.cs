using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.MessageBus.RequestResponse
{
    public interface IRequest
    {
    }

    public interface IRequest<TResponse>
        where TResponse : class
    {
    }

    //public interface IRequestWithContext<TContext> : IRequest
    //{
    //    TContext Context { get; }
    //}

    //public interface IRequestWithContext<TResponse, TContext> : IRequest<TResponse>
    //where TResponse : class
    //{
    //    TContext Context { get; }
    //}
}