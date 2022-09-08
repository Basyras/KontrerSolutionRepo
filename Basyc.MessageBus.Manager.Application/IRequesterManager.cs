using Basyc.MessageBus.Manager.Application.Initialization;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application
{
	public interface IRequestManager
	{
		RequestResult StartRequest(Request request);
		Dictionary<RequestInfo, List<RequestResult>> Results { get; }
	}
}
