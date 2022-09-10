using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public class RequestManager : IRequestManager
	{
		private readonly IRequester[] requesters;
		private readonly IRequesterSelector requesterSelector;
		private readonly IResultLoggingManager loggingManager;
		private int requestCounter;

		public RequestManager(IEnumerable<IRequester> requesters, IRequesterSelector requesterSelector, IResultLoggingManager loggingManager)
		{
			this.requesters = requesters.ToArray();
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

			var requestResult = new RequestResult(request, DateTime.Now, requestCounter++);
			results.Add(requestResult);
			var loggingContext = loggingManager.RegisterLoggingContex(requestResult);
			loggingContext.AddLog(DateTimeOffset.UtcNow, Microsoft.Extensions.Logging.LogLevel.Information, "Picking Requester");
			var requester = requesterSelector.PickRequester(request.RequestInfo);
			loggingContext.AddLog(DateTimeOffset.UtcNow, Microsoft.Extensions.Logging.LogLevel.Information, "Starting request");
			requester.StartRequest(requestResult);
			loggingContext.AddLog(DateTimeOffset.UtcNow, Microsoft.Extensions.Logging.LogLevel.Information, "Request started");




			return requestResult;
		}
	}
}
