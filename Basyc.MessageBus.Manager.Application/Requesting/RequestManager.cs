using Basyc.Diagnostics.Shared.Durations;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public class RequestManager : IRequestManager
	{
		private readonly IRequesterSelector requesterSelector;
		private readonly IRequestDiagnosticsManager requestDiagnosticsManager;
		private int requestCounter;
		private readonly ServiceIdentity requestManagerServiceIdentity = new ServiceIdentity("RequestManager");

		public RequestManager(IRequesterSelector requesterSelector, IRequestDiagnosticsManager loggingManager)
		{
			this.requesterSelector = requesterSelector;
			this.requestDiagnosticsManager = loggingManager;
		}

		public Dictionary<RequestInfo, List<RequestResult>> Results { get; } = new Dictionary<RequestInfo, List<RequestResult>>();


		public RequestResult StartRequest(Request request)
		{
			if (Results.TryGetValue(request.RequestInfo, out var results) is false)
			{
				results = new List<RequestResult>();
				Results.Add(request.RequestInfo, results);
			}

			var requester = requesterSelector.PickRequester(request.RequestInfo);
			var durationMapBuilder = new DurationMapBuilder(requestManagerServiceIdentity);
			var requestResult = new RequestResult(request, DateTime.Now, Interlocked.Increment(ref requestCounter).ToString(), durationMapBuilder);
			results.Add(requestResult);
			var loggingContext = requestDiagnosticsManager.RegisterRequest(requestResult, durationMapBuilder);
			requester.StartRequest(requestResult, new ResultLoggingContextLogger(requestManagerServiceIdentity, loggingContext));
			loggingContext.AddLog(requestManagerServiceIdentity, DateTimeOffset.UtcNow, LogLevel.Information, "Picking requester");
			loggingContext.AddLog(requestManagerServiceIdentity, DateTimeOffset.UtcNow, LogLevel.Information, "Starting request");
			loggingContext.AddLog(requestManagerServiceIdentity, DateTimeOffset.UtcNow, LogLevel.Information, "Request started");


			return requestResult;
		}
	}
}
