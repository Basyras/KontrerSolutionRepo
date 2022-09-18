using Basyc.Extensions.SignalR.Client.Tests.MethodsServerCanCall;
using Basyc.Extensions.SignalR.Client.Tests.Mocks;


namespace Basyc.Extensions.SignalR.Client.Tests
{

	public class HubListenerTests
	{
		[Fact]
		public async Task Shoud_Not_Throw_When_Receiver_Has_0_Methods()
		{
			var messageReceiver = new MethodsServerCanCall_Empty();
			var hubConnection = HubConnectionMockBuilder.Create();
			HubListener.ForwardTo(hubConnection, messageReceiver);
			await hubConnection.StartAsync();
			await hubConnection.ReceiveMessage("test", new object?[] { 1 });
		}

		[Fact]
		public async Task Test()
		{
			var messageReceiver = new MethodsServerCanCall_Numbers();
			var hubConnection = HubConnectionMockBuilder.Create();
			HubListener.ForwardTo(hubConnection, messageReceiver);
			await hubConnection.StartAsync();
			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumber), new object?[] { 1 });
			messageReceiver.LastReceivedNumber.Should().Be(1);

			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumber), new object?[] { 2 });
			messageReceiver.LastReceivedNumber.Should().Be(2);

			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumberAsync), new object?[] { 3 });
			messageReceiver.LastReceivedNumber.Should().Be(3);
		}
	}
}
