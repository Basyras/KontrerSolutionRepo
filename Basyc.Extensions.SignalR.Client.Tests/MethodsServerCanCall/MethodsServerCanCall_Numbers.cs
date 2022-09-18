namespace Basyc.Extensions.SignalR.Client.Tests.MethodsServerCanCall
{
	public class MethodsServerCanCall_Numbers
	{
		public int LastReceivedNumber { get; private set; }

		public void ReceiveNumber(int number)
		{
			LastReceivedNumber = number;
		}

		public async Task ReceiveNumberAsync(int number)
		{
			LastReceivedNumber = number;
			await Task.Delay(150);
		}

	}
}
