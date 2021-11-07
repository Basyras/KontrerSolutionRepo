using Kontrer.Shared.MessageBus.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.DomainDrivenDesign.Domain
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<TResponse> : IRequest<TResponse> where TResponse : class
    {
    }

    //public interface ICommandWithContext<TContext> : ICommand, IRequestWithContext<TContext>
    //{
    //}

    //public interface ICommandWithContext<TResponse, TContext> : ICommand<TResponse>, IRequestWithContext<TResponse, TContext>
    //    where TResponse : class
    //{
    //}
}