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
    public interface ICommandHandler<TCommand> : IConsumer<TCommand>
        where TCommand : class, ICommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommandHandler<TCommand, TResponse> : IConsumer<TCommand>
        where TCommand : class, ICommand<TResponse>
        where TResponse : class
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}