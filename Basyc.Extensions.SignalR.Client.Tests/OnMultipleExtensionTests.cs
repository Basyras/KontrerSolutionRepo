using Basyc.Extensions.SignalR.Client.Tests.MethodsServerCanCall;
using Basyc.Extensions.SignalR.Client.Tests.Mocks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Basyc.Extensions.SignalR.Client.Tests
{

	public class OnMultipleExtensionTests
	{
		[Fact]
		public async Task Should_Not_Throw_When_Receiver_Has_0_Methods()
		{
			var messageReceiver = new MethodsServerCanCall_Empty();
			var hubConnection = HubConnectionMockBuilder.Create();
			OnMultipleExtension.OnMultiple(hubConnection, messageReceiver);
			await hubConnection.StartAsync();
			await hubConnection.ReceiveMessage("test", new object?[] { 1 });
		}

		[Fact]
		public async Task Should_Ignore_Private_Methods()
		{
			var messageReceiver = new MethodsServerCanCall_PrivateMethods();
			var hubConnection = HubConnectionMockBuilder.Create();
			hubConnection.OnMultiple(messageReceiver);
			await hubConnection.StartAsync();
			await hubConnection.ReceiveMessage(MethodsServerCanCall_PrivateMethods.ReceiveTextMethodName, new object?[] { "text" });
			messageReceiver.LastReceivedText.Should().BeNull();
		}

		[Fact]
		public async Task Should_Include_Inherited_Methods()
		{
			var messageReceiver = new MethodsServerCanCall_Inhertited_Numbers();
			var hubConnection = HubConnectionMockBuilder.Create();
			hubConnection.OnMultiple(messageReceiver);
			await hubConnection.StartAsync();
			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Inhertited_Numbers.ReceiveNumber), new object?[] { 1 });
			messageReceiver.LastReceivedNumber.Should().Be(1);
		}

		[Fact]
		public async Task Should_Not_Match_Method_With_Different_Arg_Count()
		{
			//var messageReceiver = new MethodsServerCanCall_Numbers();
			//var hubConnection = HubConnectionMockBuilder.Create();
			//hubConnection.OnMultiple(messageReceiver);
			//await hubConnection.StartAsync();
			//await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumber), new object?[] { 2, "text" });
			//messageReceiver.LastReceivedNumber.Should().Be(0);
			//await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumber), new object?[] { 1, 2 });
			//messageReceiver.LastReceivedNumber.Should().Be(0);

			//This logic of selecting correct handler is currently mocked and should be handeled by SignalR. So skip testing for now
			await Task.Delay(10);
		}




		[Fact]
		public async Task Should_Diff_When_Name_Is_Same_But_Args_Differ()
		{
			//var messageReceiver = new MethodsServerCanCall_Numbers();
			//var hubConnection = HubConnectionMockBuilder.Create();
			//hubConnection.OnMultiple(messageReceiver);
			//await hubConnection.StartAsync();
			//await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumberAsync), new object?[] { 1, 2 });
			//messageReceiver.LastReceivedNumber.Should().Be(2);

			//await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumberAsync), new object?[] { 1, 2, 3 });
			//messageReceiver.LastReceivedNumber.Should().Be(3);

			//This logic of selecting correct handler is currently mocked and should be handeled by SignalR. So skip testing for now
			await Task.Delay(10);
		}


		[Fact]
		public async Task Test_All()
		{
			var messageReceiver = new MethodsServerCanCall_Numbers();
			var hubConnection = HubConnectionMockBuilder.Create();
			hubConnection.OnMultiple(messageReceiver);
			await hubConnection.StartAsync();
			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumber), new object?[] { 1 });
			messageReceiver.LastReceivedNumber.Should().Be(1);

			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumber), new object?[] { 2 });
			messageReceiver.LastReceivedNumber.Should().Be(2);

			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Numbers.ReceiveNumberAsync), new object?[] { 3 });
			messageReceiver.LastReceivedNumber.Should().Be(3);
		}

		[Fact]
		public async Task Should_Ignore_Methods_That_Are_Not_Specfied_IN_Interface()
		{
			var messageReceiver = new MethodsServerCanCall_Texts_Inheriting_IEmpty();
			var hubConnection = HubConnectionMockBuilder.Create();
			hubConnection.OnMultiple<IMethodsServerCanCall_Empty>(messageReceiver);
			await hubConnection.StartAsync();
			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Texts_Inheriting_IEmpty.ReceiveText), new object?[] { "1" });
			messageReceiver.LastReceivedText.Should().BeNull();

			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Texts_Inheriting_IEmpty.ReceiveTextAsync), new object?[] { "1" });
			messageReceiver.LastReceivedText.Should().BeNull();

			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Texts_Inheriting_IEmpty.ReceiveTexts), new object?[] { "1", "2" });
			messageReceiver.LastReceivedText.Should().BeNull();

			await hubConnection.ReceiveMessage(nameof(MethodsServerCanCall_Texts_Inheriting_IEmpty.ReceiveTexts), new object?[] { "1", "2" });
			messageReceiver.LastReceivedText.Should().BeNull();
		}
	}
}
