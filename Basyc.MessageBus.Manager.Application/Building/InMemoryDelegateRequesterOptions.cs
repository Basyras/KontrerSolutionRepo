using Basyc.MessageBus.Manager.Application.Initialization;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.Building
{
	public class InMemoryDelegateRequesterOptions 
	{
		Dictionary<RequestInfo, Action<RequestResult>> handlerMap = new();

		public void AddDelegateHandler(RequestInfo requestInfo, Action<RequestResult> handler)
		{
			handlerMap.Add(requestInfo,handler);
		}

		public Dictionary<RequestInfo, Action<RequestResult>> ResolveHandlers()
		{
			return handlerMap;
		}

	}
}