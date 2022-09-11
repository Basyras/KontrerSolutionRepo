using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public class InMemoryDelegateRequester : IRequester
	{
		public const string InMemoryDelegateRequesterUniqueName = nameof(InMemoryDelegateRequester);

		private readonly IOptions<InMemoryDelegateRequesterOptions> options;
		private readonly InMemoryLogSource inMemoryLogSource;
		private readonly Dictionary<RequestInfo, Action<RequestResult>> handlersMap;
		public string UniqueName => InMemoryDelegateRequesterUniqueName;

		public InMemoryDelegateRequester(IOptions<InMemoryDelegateRequesterOptions> options, InMemoryLogSource inMemoryLogSource)
		{
			this.options = options;
			this.inMemoryLogSource = inMemoryLogSource;
			handlersMap = options.Value.ResolveHandlers();
		}

		public void StartRequest(RequestResult requestResult)
		{
			//var requesterStarted = requestResult.StartNewSegment("Requester started");

			inMemoryLogSource.PushLog(requestResult.Id, LogLevel.Information, "Starting invoking in-memory delegate");
			var handler = handlersMap[requestResult.Request.RequestInfo];
			try
			{
				//var invokingSement = requestResult.StartNewSegment("Invkoking in-Memory delegate");
				handler.Invoke(requestResult);
				inMemoryLogSource.PushLog(requestResult.Id, LogLevel.Information, "In-memory delegate completed");

			}
			catch (Exception ex)
			{
				inMemoryLogSource.PushLog(requestResult.Id, LogLevel.Error, "In-memory delegate failed");
				requestResult.Fail(ex.Message);
			}
		}

		public void AddHandler(RequestInfo requestInfo, Action<RequestResult> handler)
		{
			handlersMap.Add(requestInfo, handler);
		}
	}
}