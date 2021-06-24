using Kontrer.Shared.DomainDrivenDesign.Domain;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.Shared.DomainDrivenDesign.Application
{
    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        public Task Consume(ConsumeContext<TCommand> context)
        {
            return Handle(context.Message);
        }

        public abstract Task Handle(TCommand command, CancellationToken cancellationToken = default);
    }

    public abstract class CommandHandlerBase<TCommand, TReponse> : ICommandHandler<TCommand, TReponse>
        where TReponse : class
        where TCommand : class, ICommand<TReponse>
    {
        public Task Consume(ConsumeContext<TCommand> context)
        {
            return Handle(context.Message);
        }

        public abstract Task<TReponse> Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}