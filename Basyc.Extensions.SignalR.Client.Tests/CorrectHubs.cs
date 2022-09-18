namespace Basyc.Extensions.SignalR.Client.Tests
{
	public static class CorrectHubs
	{
		public static Type[] CorrectHubTypes = new Type[]
		{
			typeof(ICorrectHubClientWithoutMethods),
			typeof(ICorrectHubClientWithAllMethodTypes),
			typeof(ICorrectHubClient3),
		};
	}

	public interface ICorrectHubClientWithoutMethods
	{

	}

	public interface ICorrectHubClientWithAllMethodTypes
	{
		void SendNothing();
		void SendNumber(int number);
		Task SendNumberAsync(int number);
		Task SendNumberAsync(int number, CancellationToken cancellationToken);
	}

	public interface ICorrectHubClient3
	{
		Task SendNumberAsync(int number, int number2, int number3, int number4, CancellationToken cancellationToken);
	}
}