using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus
{

    public abstract class MessageBusManagerBase : IMessageBusManager
    {
        public IReadOnlyCollection<BusSubscription> BusSubscriptions { get => tempSubscriptions.AsReadOnly(); }
        protected List<BusSubscription> tempSubscriptions;
        public bool IsSubscriptionLocked { get; protected set; } = false;

        public abstract string BusName { get; }

        public MessageBusManagerBase()
        {
            tempSubscriptions = new();
        }



        public virtual void LockSubscriptions()
        {
            IsSubscriptionLocked.Should().BeFalse("Can't lock subcriptions twice");
            IsSubscriptionLocked = true;
        }

        public abstract void RegisterSubscribe<TRequest, TResponse>(Func<TRequest, Task> asyncHandler, string topicName = null);
        public abstract Task PublishAsync<TRequest>(TRequest data, CancellationToken cancellationToken = default, string topicName = null);


        public abstract void RegisterPull<TRequest>(Action<TRequest> pullHandler);
        public abstract Task PushAsync<TRequest>(TRequest request);

        public abstract Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;
        public abstract Task RequestAsync<TRequest>(TRequest request) where TRequest : IRequest;
    }
}
