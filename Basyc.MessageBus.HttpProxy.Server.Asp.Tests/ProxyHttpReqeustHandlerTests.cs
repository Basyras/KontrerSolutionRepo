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

namespace Basyc.MessageBus.HttpProxy.Server.Asp.Tests
{
    public class ProxyHttpReqeustHandlerTests
    {
        private readonly Mock<ITypedMessageBusClient> messageBusMock;
        private readonly ProxyHttpReqeustHandler handler;
        private readonly Mock<HttpContext> httpContextMock;
        private readonly IRequestSerializer serializer;

        public ProxyHttpReqeustHandlerTests()
        {
            messageBusMock = new Mock<ITypedMessageBusClient>();
            handler = new ProxyHttpReqeustHandler(messageBusMock.Object, new JsonRequestSerializer());
            httpContextMock = new Mock<HttpContext>();
            serializer = new JsonRequestSerializer();
        }

        [Fact]
        public async Task Throws_When_MessageBus_Throws()
        {
            var proxyRequest = ProxyRequest.Create(typeof(DummyRequest), serializer.Serialize(new DummyRequest()));
            var proxyBytes = JsonSerializer.SerializeToUtf8Bytes(proxyRequest);
            var proxyMemory = new MemoryStream(proxyBytes);

            httpContextMock.SetupGet((x) => x.Request.Body).Returns(proxyMemory);

            string busErrorMessage = "BUS_ERROR_MESSAGE";
            messageBusMock
                .Setup(x => x.SendAsync(It.IsAny<Type>(), It.IsAny<DummyRequest>(), default))
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