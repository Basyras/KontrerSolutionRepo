using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.MessageBus.Client;
using Basyc.MessageBus.Shared;
using Basyc.Serializaton.Abstraction;
using Basyc.Serialization.Abstraction;
using Basyc.Serailization.SystemTextJson;

namespace Basyc.MessageBus.HttpProxy.Server.Asp.Tests
{
    public class ProxyHttpReqeustHandlerTests
    {
        private readonly Mock<ISimpleMessageBusClient> messageBusMock;
        private readonly ProxyHttpReqeustHandler handler;
        private readonly Mock<HttpContext> httpContextMock;
        private readonly ISimpleByteSerailizer serializer;

        public ProxyHttpReqeustHandlerTests()
        {
            messageBusMock = new Mock<ISimpleMessageBusClient>();
            var serializer = new SimpleFromTypedSerializer(new JsonByteSerializer());
            handler = new ProxyHttpReqeustHandler(messageBusMock.Object, serializer);
            httpContextMock = new Mock<HttpContext>();
        }

        [Fact]
        public async Task Throws_When_MessageBus_Throws()
        {
            var dummyRequestType = TypedToSimpleConverter.ConvertTypeToSimple<DummyRequest>();
            var ser = serializer.Serialize(new DummyRequest(), dummyRequestType);
            var proxyRequest = new ProxyRequest(dummyRequestType, dummyRequestType);
            
            var proxyBytes = JsonSerializer.SerializeToUtf8Bytes(proxyRequest);
            var proxyMemory = new MemoryStream(proxyBytes);

            httpContextMock.SetupGet((x) => x.Request.Body).Returns(proxyMemory);

            string busErrorMessage = "BUS_ERROR_MESSAGE";
            messageBusMock
                .Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<DummyRequest>(), default))
                .Throws(new Exception(busErrorMessage));

            Func<Task> taskWrapper = async () =>
            {
                await handler.Handle(httpContextMock.Object);
            };

            await taskWrapper.Should().ThrowAsync<Exception>().WithMessage($"*{busErrorMessage}*");
        }
    }

    public record DummyRequest : IMessage;
}