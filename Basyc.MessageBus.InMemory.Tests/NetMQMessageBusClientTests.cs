using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.NetMQ;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Basyc.MessageBus.InMemory.Tests
{
    public class NetMQMessageBusClientTests
    {
        IMessageBusClient client;
        

        public NetMQMessageBusClientTests()        
        {
            var mock = new Mock<ILogger<NetMQMessageBusClient>>();
            ILogger<NetMQMessageBusClient> logger = mock.Object;
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