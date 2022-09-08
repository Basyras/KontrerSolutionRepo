using Basyc.MessageBus.Manager.Application.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application
{
	public class RequestManager : IRequestManager
	{
		private readonly IRequester[] requesters;
		private readonly IRequesterSelector requesterSelector;

		public RequestManager(IEnumerable<IRequester> requesters, IRequesterSelector requesterSelector)
		{
			this.requesters = requesters.ToArray();
			this.requesterSelector = requesterSelector;
		}

		public Dictionary<RequestInfo, List<RequestResult>> Results { get; } = new Dictionary<RequestInfo, List<RequestResult>>();


		public RequestResult StartRequest(Request request)
		{
			if (Results.TryGetValue(request.RequestInfo, out var results) is false)
			{
				results = new List<RequestResult>();
				Results.Add(request.RequestInfo, results);
			}

			var requestResult = new RequestResult(request, DateTime.Now, results.Count);
			results.Add(requestResult);

			var requester = requesterSelector.PickRequester(request.RequestInfo);
			requester.StartRequest(requestResult);
			return requestResult;
		}
	}
}
