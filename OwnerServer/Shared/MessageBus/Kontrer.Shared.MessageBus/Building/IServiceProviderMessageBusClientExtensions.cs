using Basyc.MessageBus.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceProviderMessageBusClientExtensions
    {
        public static Task StartMessageBusClientAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            var busClient = serviceProvider.GetRequiredService<IObjectMessageBusClient>();
            return busClient.StartAsync(cancellationToken);
        }
    }
}
