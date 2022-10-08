using Basyc.MessageBus.Manager.Application.Initialization;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public class InMemoryDelegateRequesterOptions
	{
		private readonly Dictionary<RequestInfo, Action<RequestResultContext>> handlerMap = new();

		public void AddDelegateHandler(RequestInfo requestInfo, Action<RequestResultContext> handler)
		{
			handlerMap.Add(requestInfo, handler);
		}

		public Dictionary<RequestInfo, Action<RequestResultContext>> ResolveHandlers()
		{
			return handlerMap;
		}

	}
}