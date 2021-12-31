using Basyc.MessageBus.Shared;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
    public interface ISimpleMessageBusClient : IDisposable
    {
        Task PublishAsync(string eventType, CancellationToken cancellationToken = default);
        Task PublishAsync(string eventType, object eventData, CancellationToken cancellationToken = default);

        Task SendAsync(string commandType, CancellationToken cancellationToken = default);
        Task SendAsync(string commandType, object commandData, CancellationToken cancellationToken = default);

        Task<object> RequestAsync(string requestType, CancellationToken cancellationToken = default);
        Task<OneOf<object,ErrorMessage>> RequestAsync(string requestType, object requestData, CancellationToken cancellationToken = default);

        Task StartAsync(CancellationToken cancellationToken = default);
    }
}
