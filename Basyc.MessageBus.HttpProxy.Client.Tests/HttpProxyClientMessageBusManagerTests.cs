using FluentAssertions;
using Kontrer.Shared.MessageBus.HttpProxy.Shared;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Contrib.HttpClient;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Basyc.MessageBus.HttpProxy.Client.Tests
{
    public class HttpProxyClientMessageBusManagerTests
    {
        private readonly ProxyObjectMessageBusClient manager;
        private readonly Mock<HttpMessageHandler> httpHandlerMock;

        public HttpProxyClientMessageBusManagerTests()
        {
            httpHandlerMock = new Mock<HttpMessageHandler>();
            var client = httpHandlerMock.CreateClient();
            var serilizer = new JsonRequestSerializer();
            var options = Options.Create(new ProxyObjectMessageBusClientOptions() { ProxyHostUri = new Uri("https://localhost:6969/") });
            manager = new HttpProxyClientMessageBusManager(options, client, serilizer);
        }

        //[Fact]
        //public async Task Should_Send_On_Valid()
        //{
        //    var responseContent =

        //    httpHandlerMock.SetupAnyRequest()
        //        .ReturnsResponse(System.Net.HttpStatusCode.OK, new StringContent("TestCon"));

        //    await manager.SendAsync(typeof(object), new object(), default);
        //}

        [Fact]
        public async Task Should_Throw_On_Bad_Status_Code()
        {
            var serverErrorMessage = "CustomServerMessage";
            httpHandlerMock.SetupAnyRequest()
                .ReturnsResponse(System.Net.HttpStatusCode.BadRequest, x => x.ReasonPhrase = serverErrorMessage);

            Func<Task> f = async () =>
            {
                await manager.SendAsync(typeof(object), new object(), default);
            };
            await f.Should().ThrowAsync<Exception>().WithMessage($"*{serverErrorMessage}*");
        }

        [Fact]
        public async Task Should_Throw_On_Inner_Exception()
        {
            var exceptionMessage = "ERROR";
            httpHandlerMock.SetupAnyRequest().Throws(new Exception(exceptionMessage));
            Func<Task> f = async () =>
            {
                await manager.SendAsync(typeof(object), new object(), default);
            };
            await f.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }
    }
}