using Basyc.MessageBus.Manager.Application.Initialization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.Building
{
	public class InMemoryDelegateRequester : IRequester
	{
		public const string InMemoryDelegateRequesterUniqueName = nameof(InMemoryDelegateRequester);

		private readonly IOptions<InMemoryDelegateRequesterOptions> options;
		private readonly Dictionary<RequestInfo, Action<RequestResult>> handlersMap;

		public InMemoryDelegateRequester(IOptions<InMemoryDelegateRequesterOptions> options)
		{
			this.options = options;
			handlersMap = options.Value.ResolveHandlers();
		}

		public string UniqueName => InMemoryDelegateRequesterUniqueName;


		public void StartRequest(RequestResult requestResult)
		{
			var handler = handlersMap[requestResult.Request.RequestInfo];
			handler.Invoke(requestResult);
		}

		public void AddHandler(RequestInfo requestInfo, Action<RequestResult> handler)
		{
			handlersMap.Add(requestInfo, handler);
		}
	}
}