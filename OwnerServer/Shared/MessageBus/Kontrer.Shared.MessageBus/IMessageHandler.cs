using Basyc.MessageBus.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.RequestResponse
{
    public interface IMessageHandler<TMessage> where TMessage : IMessage
    {
        Task Handle(TMessage message, EventId eventId, CancellationToken cancellationToken = default);
    }

    public interface IMessageHandler<TMessage, TResponse>
        where TMessage : IMessage<TResponse>
        where TResponse : class
    {
        Task<TResponse> Handle(TMessage message, EventId eventId, CancellationToken cancellationToken = default);
    }
}