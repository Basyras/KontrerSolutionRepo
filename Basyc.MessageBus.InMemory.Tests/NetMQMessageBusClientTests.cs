using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.NetMQ;
using Basyc.MessageBus.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Basyc.MessageBus.InMemory.Tests
{
    public class NetMQMessageBusClientTests
    {
        ITypedMessageBusClient client;
        

        public NetMQMessageBusClientTests()        
        {
            var mock = new Mock<ILogger<NetMQSimpleMessageBusClient>>();
            ILogger<NetMQSimpleMessageBusClient> logger = mock.Object;
            client = new NetMQMessageBusClient(logger);
        }

        [Fact]
        public async Task Work()
        {
            var res = await client.RequestAsync<TestRequest, Customer>();
        }
    }

    public record TestRequest : IMessage<Customer>;
    public record Customer;
}