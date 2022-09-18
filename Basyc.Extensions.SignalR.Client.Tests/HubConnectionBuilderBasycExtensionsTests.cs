using Basyc.Extensions.SignalR.Client.Tests.Mocks;

namespace Basyc.Extensions.SignalR.Client.Tests
{
	public partial class HubConnectionBuilderBasycExtensionsTests
	{

		[Fact]
		public void When_BuildStrongTyped_With_CorrectHub_Should_Not_Throw()
		{
			var hubClient = new HubConnectionMockBuilder()
			.BuildStrongTyped<ICorrectHubClientWithAllMethodTypes>();

			var mockConnection = (HubConnectionMock)hubClient.UnderlyingHubConnection;
			hubClient.Methods.SendNumber(1);

			mockConnection.LastSendCoreCall.Should().NotBeNull();
			mockConnection.LastSendCoreCall!.MethodName.Should().Be(nameof(ICorrectHubClientWithAllMethodTypes.SendNumber));
		}

		[Fact]
		public async Task When_CreateStrongTyped_With_CorrectHub_Should_Not_Throw()
		{
			var connectionMock = new HubConnectionMockBuilder().BuildAsMock();
			var hubClient = connectionMock.CreateStrongTyped<ICorrectHubClientWithAllMethodTypes>();

			hubClient.Methods.SendNumber(1);
			connectionMock.LastSendCoreCall.Should().NotBeNull();
			connectionMock.LastSendCoreCall!.MethodName.Should().Be(nameof(ICorrectHubClientWithAllMethodTypes.SendNumber));
			connectionMock.LastSendCoreCall!.Args.Should().Equal(new object?[] { 1 });

			hubClient.Methods.SendNothing();
			connectionMock.LastSendCoreCall!.MethodName.Should().Be(nameof(ICorrectHubClientWithAllMethodTypes.SendNothing));
			connectionMock.LastSendCoreCall!.Args.Should().Equal(new object?[] { });

			var sendNumberTask = hubClient.Methods.SendNumberAsync(2);
			sendNumberTask.Should().NotBeNull();
			sendNumberTask.Should().BeOfType<Task>();
			connectionMock.LastSendCoreCall!.MethodName.Should().Be(nameof(ICorrectHubClientWithAllMethodTypes.SendNumberAsync));
			connectionMock.LastSendCoreCall!.Args.Should().Equal(new object?[] { 2 });



		}
	}
}