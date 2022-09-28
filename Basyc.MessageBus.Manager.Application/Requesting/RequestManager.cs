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
		private readonly IResultLoggingManager loggingManager;
		private int requestCounter;

		public RequestManager(IRequesterSelector requesterSelector, IResultLoggingManager loggingManager)
		{
			this.requesterSelector = requesterSelector;
			this.loggingManager = loggingManager;
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
			var durationMapBuilder = new DurationMapBuilder();
			var requestResult = new RequestResult(request, DateTime.Now, Interlocked.Increment(ref requestCounter), durationMapBuilder);
			results.Add(requestResult);
			var loggingContext = loggingManager.RegisterLoggingContex(requestResult);
			requester.StartRequest(requestResult, new ResultLoggingContextLogger(loggingContext));
			loggingContext.AddLog(DateTimeOffset.UtcNow, LogLevel.Information, "Picking requester");
			loggingContext.AddLog(DateTimeOffset.UtcNow, LogLevel.Information, "Starting request");
			loggingContext.AddLog(DateTimeOffset.UtcNow, LogLevel.Information, "Request started");


			return requestResult;
		}
	}
}
