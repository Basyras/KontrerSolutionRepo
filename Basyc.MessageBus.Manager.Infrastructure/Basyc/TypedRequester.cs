using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using Basyc.MessageBus.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
	public class TypedRequester : IRequester
	{
		private readonly IRequestInfoTypeStorage requestInfoTypeStorage;
		private readonly IResponseFormatter responseFormatter;

		public TypedRequester(ITypedMessageBusClient messageBusManager, IRequestInfoTypeStorage requestInfoTypeStorage, IResponseFormatter responseFormatter)
		{
			MessageBusManager = messageBusManager;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
			this.responseFormatter = responseFormatter;
		}

		public ITypedMessageBusClient MessageBusManager { get; }

		public Dictionary<RequestInfo, List<RequestResult>> Results { get; } = new Dictionary<RequestInfo, List<RequestResult>>();

		public RequestResult StartRequest(Request request)
		{
			var requestType = requestInfoTypeStorage.GetRequestType(request.RequestInfo);
			var paramValues = request.Parameters.Select(x => x.Value).ToArray();
			var requestObject = Activator.CreateInstance(requestType, paramValues);
			if (Results.TryGetValue(request.RequestInfo, out var results) is false)
			{
				results = new List<RequestResult>();
				Results.Add(request.RequestInfo, results);
			}
			var result = new RequestResult(request, DateTime.Now, results.Count);
			results.Add(result);

			Task.Run(async () =>
			{
				var stopWatch = new Stopwatch();
				try
				{
					stopWatch.Start();
					if (request.RequestInfo.HasResponse)
					{
						var response = await MessageBusManager.RequestAsync(requestType, requestObject, request.RequestInfo.ResponseType);
						stopWatch.Stop();

						if (response.Value is ErrorMessage error)
						{
							result.Fail(stopWatch.Elapsed, error.Message);
						}
						else
						{
							var resultObject = response.AsT0;
							result.Complete(stopWatch.Elapsed, responseFormatter.Format(resultObject));
						}
					}
					else
					{
						await MessageBusManager.SendAsync(requestType, requestObject)
						.ContinueWith(x =>
						{
							stopWatch.Stop();
							string errorMessage = string.Empty;
							if (x.IsFaulted)
							{
								if (x.Exception is AggregateException aggregateException)
								{
									errorMessage = aggregateException.InnerExceptions.Select(x => x.Message).Aggregate((x, y) => $"{x},\n{y}");
								}
								else
								{
									errorMessage = x.Exception != null ? x.Exception.Message : string.Empty;
								}
								result.Fail(stopWatch.Elapsed, errorMessage);
							}
							result.Complete(stopWatch.Elapsed);
						});
					}
				}
				catch (Exception ex)
				{
					stopWatch.Stop();
					result.Fail(stopWatch.Elapsed, ex.Message);
				}
			});
			return result;
		}
	}
}