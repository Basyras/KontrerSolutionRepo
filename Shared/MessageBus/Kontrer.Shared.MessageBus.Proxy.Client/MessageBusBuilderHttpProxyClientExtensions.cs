using Basyc.MessageBus.Client.Building;
using Basyc.MessageBus.HttpProxy.Client;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MessageBusBuilderHttpProxyClientExtensions
    {
        public static MessageBusHttpProxyClientBuilder AddProxyClient(this BusClientSelectMessageTypeStage builder)
        {
            builder.services.AddBasycSerialization().SelectProtobufNet();
            builder.services.AddSingleton(new HttpClient());
            return new MessageBusHttpProxyClientBuilder(builder.services);
        }
    }
}