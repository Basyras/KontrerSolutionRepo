using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus
{
    public interface IMessageBusManager
    {  
        //void RegisterSubscribe<TResponse>(Func<TResponse, Task> asyncHandler, string topicName = null);

        Task PublishAsync<TRequest>(TRequest data, CancellationToken cancellationToken = default, string topicName = null)
              where TRequest : class;

        //void RegisterHandler<TRequest>(MediatR.AsyncRequestHandler<TRequest> handler) where TRequest : IRequest;

        //void RegisterHandler<THandler, TRequest>()
        //    where THandler : IRequestHandler<TRequest>
        //    where TRequest : IRequest<Unit>;


        void RegisterConsumer<IConsumer>();

        Task RequestAsync<TRequest>(CancellationToken cancellationToken = default)
          where TRequest : class, IRequest, new();

        Task RequestAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest;

        Task<TResponse> RequestAsync<TRequest, TResponse>(CancellationToken cancellationToken = default)
             where TRequest : class, IRequest<TResponse>, new()
             where TResponse : class;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class;

       





    }
}
