using Kontrer.Shared.MessageBus.RequestResponse;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.DomainDrivenDesign.Application
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