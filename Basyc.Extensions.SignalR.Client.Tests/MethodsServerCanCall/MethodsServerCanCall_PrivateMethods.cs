namespace Basyc.Extensions.SignalR.Client.Tests.MethodsServerCanCall
{
	public class MethodsServerCanCall_PrivateMethods
	{
		public string? LastReceivedText { get; private set; }
		public const string ReceiveTextMethodName = nameof(ReceiveText);
		private void ReceiveText(string text)
		{
			LastReceivedText = text;
		}
	}
}
