namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public interface IRequester
	{
		string UniqueName { get; }
		void StartRequest(RequestResult requestResult);
	}
}