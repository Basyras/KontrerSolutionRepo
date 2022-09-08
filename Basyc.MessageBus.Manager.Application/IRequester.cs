namespace Basyc.MessageBus.Manager.Application
{
	public interface IRequester
	{
		string UniqueName { get; }
		void StartRequest(RequestResult requestResult);
	}
}