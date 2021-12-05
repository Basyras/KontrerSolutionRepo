using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Basyc.DomainDrivenDesign.Domain;
using Basyc.MessageBus.RequestResponse;

namespace Basyc.DomainDrivenDesign.Application
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
        where TCommand : class, ICommand
    {
    }

    public interface ICommandHandler<TCommand, TReponse> : IRequestHandler<TCommand, TReponse>
        where TCommand : class, ICommand<TReponse>
        where TReponse : class
    {
    }
}