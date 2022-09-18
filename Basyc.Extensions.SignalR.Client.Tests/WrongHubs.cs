namespace Basyc.Extensions.SignalR.Client.Tests
{
	public static class WrongHubs
	{
		public static Type[] WrongHubTypes = new Type[]
		{
			typeof(IWrongHubClient_Has_ReturnValue),
			typeof(IWrongHubClient_Has_TaskReturnValue),
			typeof(IWrongHubClient_Has_MultipleReturnValue),
		};
	}

	public interface IWrongHubClient_Has_ReturnValue
	{
		int SendNothingReceiveNumber();
	}

	public interface IWrongHubClient_Has_TaskReturnValue
	{
		Task<int> SendNothingReceiveTaskNumber();
	}

	public interface IWrongHubClient_Has_MultipleReturnValue
	{
		int SendNothing();
		int SendNothing2();
		string SendNothing3(int number);
		object SendNothing4(int number, string name);
	}

	public interface IWrongHubClient_Has_AllWrongs :
		IWrongHubClient_Has_ReturnValue,
		IWrongHubClient_Has_TaskReturnValue,
		IWrongHubClient_Has_MultipleReturnValue
	{
	}
}