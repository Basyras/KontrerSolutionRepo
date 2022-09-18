using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Basyc.Extensions.SignalR.Client.Tests.Mocks
{
	public class HubConnectionMockBuilder : IHubConnectionBuilder
	{
		public HubConnectionMockBuilder()
		{

		}
		public IServiceCollection Services => throw new NotImplementedException();

		public HubConnection Build()
		{
			return Create();
		}

		public HubConnectionMock BuildAsMock()
		{
			return Create();
		}

		public static HubConnectionMock Create()
		{
			var connectionFactoryMock = new Mock<IConnectionFactory>();
			var hubProtocolMock = new Mock<IHubProtocol>();
			var endpoint = new IPEndPoint(0, 0);
			var servicProviderMock = new Mock<IServiceProvider>();
			var loggerFactoryMock = new Mock<ILoggerFactory>();
			var retryPolicyMock = new Mock<IRetryPolicy>();
			return new HubConnectionMock(connectionFactoryMock.Object, hubProtocolMock.Object, endpoint, servicProviderMock.Object, loggerFactoryMock.Object, retryPolicyMock.Object);
		}
	}
}
