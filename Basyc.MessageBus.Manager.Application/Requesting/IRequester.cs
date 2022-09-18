namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public interface IRequester
	{
		string UniqueName { get; }
		/// <summary>
		/// Returns session id of the request
		/// </summary>
		/// <param name="requestResult"></param>
		/// <returns></returns>
		void StartRequest(RequestResult requestResult);
	}
}