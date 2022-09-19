using Basyc.Extensions.SignalR.Client.Tests.Mocks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Basyc.Extensions.SignalR.Client.Tests
{
	public partial class HubConnectionBuilderBasycExtensionsTests
	{

		[Fact]
		public void When_BuildStrongTyped_With_CorrectHub_Should_Not_Throw()
		{
			var hubClient = new HubConnectionMockBuilder()
				.BuildStrongTyped<ICorrectHubClient_Has_Voids>();

			var mockConnection = (HubConnectionMock)hubClient.UnderlyingHubConnection;
			hubClient.Call.SendNumber(1);

			mockConnection.LastSendCoreCall.Should().NotBeNull();
			mockConnection.LastSendCoreCall!.MethodName.Should().Be(nameof(ICorrectHubClient_Has_Voids.SendNumber));
		}

		[Fact]
		public void When_CreateStrongTyped_With_CorrectHub_Should_Not_Throw()
		{
			var connectionMock = new HubConnectionMockBuilder().BuildAsMock();
			var hubClient = connectionMock.CreateStrongTyped<ICorrectHubClient_Has_AllCorrect>();

			hubClient.Call.SendNumber(1);
			connectionMock.LastSendCoreCall.Should().NotBeNull();
			connectionMock.LastSendCoreCall!.MethodName.Should().Be(nameof(ICorrectHubClient_Has_AllCorrect.SendNumber));
			connectionMock.LastSendCoreCall!.Args.Should().Equal(new object?[] { 1 });

			hubClient.Call.SendNothing();
			connectionMock.LastSendCoreCall!.MethodName.Should().Be(nameof(ICorrectHubClient_Has_AllCorrect.SendNothing));
			connectionMock.LastSendCoreCall!.Args.Should().Equal(Array.Empty<object?>());

			var sendNumberTask = hubClient.Call.SendIntAsync(2);
			sendNumberTask.Should().NotBeNull();
			sendNumberTask.Should().BeAssignableTo<Task>();
			connectionMock.LastSendCoreCall!.MethodName.Should().Be(nameof(ICorrectHubClient_Has_AllCorrect.SendIntAsync));
			connectionMock.LastSendCoreCall!.Args.Should().Equal(new object?[] { 2 });



		}
	}
}