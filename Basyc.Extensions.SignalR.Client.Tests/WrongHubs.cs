namespace Basyc.Extensions.SignalR.Client.Tests
{
	public static class WrongHubs
	{
		public static Type[] WrongHubTypes = new Type[]
		{
			typeof(IWrongHubClient_Has_ReturnValues),
			typeof(IWrongHubClient_Has_TaskReturnValues),
			typeof(IWrongHubClient_HasInherited_ReturnValue),
			typeof(IWrongHubClient_Has_AllWrongs),
		};
	}

	public interface IWrongHubClient_Has_ReturnValues : ICorrectHubClient_Has_Voids
	{
		int WrongSendNothingReceiveNumber();
		string WrongSendNothingReceiveText();
		int WrongSendNothing();
		int WrongSendNothing2();
		string WrongSendInt(int number);
		object WrongSendIntString(int number, string name);
	}

	public interface IWrongHubClient_Has_TaskReturnValues : ICorrectHubClient_Has_Voids
	{
		Task<int> WrongSendNothingAsyncInt();
		Task<int> WrongSendIntAsyncInt(int number);
		Task<int> WrongSendIntCancelAsyncInt(int number, CancellationToken cancellationToken);
		Task<int> WrongSendIntStringAsyncInt(int number, string text);
	}

	public interface IWrongHubClient_HasInherited_ReturnValue : IWrongHubClient_Has_ReturnValues
	{
	}

	public interface IWrongHubClient_Has_AllWrongs :
		IWrongHubClient_Has_ReturnValues,
		IWrongHubClient_Has_TaskReturnValues,
		ICorrectHubClient_Has_AllCorrect
	{
	}
}