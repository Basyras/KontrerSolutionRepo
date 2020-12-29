using FluentAssertions;
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

        public abstract Task PublishAsync<TRequest>(TRequest data, CancellationToken cancellationToken = default, string topicName = null);


        public virtual void LockSubscriptions()
        {
            IsSubscriptionLocked.Should().BeFalse("Can't lock subcriptions twice");
            IsSubscriptionLocked = true;
        }

        public abstract void Subscribe<TRequest, TResponse>(Func<TRequest, Task> asyncHandler, string topicName = null);
        
          
        
    }
}
