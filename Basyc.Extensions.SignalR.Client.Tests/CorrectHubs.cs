namespace Basyc.Extensions.SignalR.Client.Tests
{
	public static class CorrectHubs
	{
		public static Type[] CorrectHubTypes = new Type[]
		{
			typeof(ICorrectHubClient_Has_NoMethods),
			typeof(ICorrectHubClient_Has_Voids),
			typeof(ICorrectHubClient_Has_Tasks),
			typeof(ICorrectHubClient_HasInherited_Voids),
			typeof(ICorrectHubClient_Has_AllCorrect),
		};
	}

	public interface ICorrectHubClient_Has_NoMethods
	{

	}

	public interface ICorrectHubClient_Has_Voids
	{
		void SendNothing();
		void SendNumber(int number);
		void SendIntString(int number, string name);
	}

	public interface ICorrectHubClient_Has_Tasks
	{
		Task SendNothingAsync();
		Task SendIntAsync(int number);
		Task SendIntCancelAsync(int number, CancellationToken cancellationToken);
		Task SendIntStringCancelAsync(int number, string name, CancellationToken cancellationToken);
	}

	public interface ICorrectHubClient_HasInherited_Voids : ICorrectHubClient_Has_Voids
	{

	}


	public interface ICorrectHubClient_Has_AllCorrect :
		ICorrectHubClient_Has_NoMethods,
		ICorrectHubClient_Has_Voids,
		ICorrectHubClient_Has_Tasks
	{
	}

}
