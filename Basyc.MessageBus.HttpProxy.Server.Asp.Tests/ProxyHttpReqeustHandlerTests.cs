using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared;
using Basyc.MessageBus.Shared;
using Basyc.Serailization.SystemTextJson;
using Basyc.Serialization.Abstraction;
using Basyc.Serializaton.Abstraction;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Basyc.MessageBus.HttpProxy.Server.Asp.Tests
{
	public class ProxyHttpReqeustHandlerTests
	{
		private readonly Mock<IByteMessageBusClient> messageBusMock;
		private readonly ProxyHttpReqeustHandler handler;
		private readonly Mock<HttpContext> httpContextMock;
		private readonly ISimpleToByteSerailizer serializer;

		public ProxyHttpReqeustHandlerTests()
		{
			messageBusMock = new Mock<IByteMessageBusClient>();
			var serializer = new SimpleFromTypedByteSerializer(new JsonByteSerializer());
			handler = new ProxyHttpReqeustHandler(messageBusMock.Object, serializer);
			httpContextMock = new Mock<HttpContext>();
		}

		[Fact]
		public async Task Throws_When_MessageBus_Throws()
		{
			var dummyRequestType = TypedToSimpleConverter.ConvertTypeToSimple<DummyRequest>();
			var ser = serializer.Serialize(new DummyRequest(), dummyRequestType);
			var proxyRequest = new ProxyRequest(dummyRequestType, false);

			var proxyBytes = JsonSerializer.SerializeToUtf8Bytes(proxyRequest);
			var proxyMemory = new MemoryStream(proxyBytes);

			httpContextMock.SetupGet((x) => x.Request.Body).Returns(proxyMemory);

			string busErrorMessage = "BUS_ERROR_MESSAGE";
			messageBusMock
				.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<byte[]>(), default))
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