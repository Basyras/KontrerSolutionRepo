using Basyc.MessageBus.Manager.Application.Initialization;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public interface IRequestManager
	{
		RequestResultContext StartRequest(Request request);
		Dictionary<RequestInfo, List<RequestResultContext>> Results { get; }
	}
}
