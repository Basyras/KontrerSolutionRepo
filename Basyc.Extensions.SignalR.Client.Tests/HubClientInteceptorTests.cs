using Castle.DynamicProxy;
using Microsoft.AspNetCore.SignalR.Client;

namespace Basyc.Extensions.SignalR.Client.Tests
{
	public class HubClientInteceptorTests
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

		private readonly HubConnection connection;
		public HubClientInteceptorTests()
		{
			connection = new HubConnectionBuilder().WithUrl("https://localhost:44310/correctHub").Build();
		}

		[Fact]
		public void Should_Create_Proxy_With_0_Methods()
		{
			var inteceptor = new HubClientInteceptor(connection, typeof(ICorrectHubClientWithoutMethods));
			var proxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<ICorrectHubClientWithoutMethods>(inteceptor);
		}


		[Fact]
		public void Should_Create_Proxy_With_All_Method_Types()
		{
			var inteceptor = new HubClientInteceptor(connection, typeof(ICorrectHubClientWithAllMethodTypes));
			var proxy = proxyGenerator.CreateInterfaceProxyWithoutTarget<ICorrectHubClientWithAllMethodTypes>(inteceptor);
		}

		[Fact]
		public void Should_Throw_When_CreatingWrongHub()
		{
			foreach (var hubType in WrongHubs.WrongHubTypes)
			{
				var action = () =>
				{
					var inteceptor = new HubClientInteceptor(connection, hubType);
				};
				action.Should().Throw<ArgumentException>();

			}

		}

		[Fact]
		public void Should_Not_Throw_When_CreatingCorrectHub()
		{
			foreach (var hubType in CorrectHubs.CorrectHubTypes)
			{
				var action = () =>
				{
					var inteceptor = new HubClientInteceptor(connection, hubType);
				};
				action.Should().NotThrow();

			}

		}
	}
}
