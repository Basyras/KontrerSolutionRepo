using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Requesting;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using Basyc.MessageBus.Shared;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
	public class BasycTypedMessageBusRequester : IRequester
	{
		public const string BasycTypedMessageBusRequesterUniqueName = nameof(BasycTypedMessageBusRequester);


		private readonly IRequestInfoTypeStorage requestInfoTypeStorage;
		private readonly IResponseFormatter responseFormatter;
		private readonly ITypedMessageBusClient typedMessageBusClient;

		public BasycTypedMessageBusRequester(ITypedMessageBusClient typedMessageBusClient, IRequestInfoTypeStorage requestInfoTypeStorage, IResponseFormatter responseFormatter)
		{
			this.typedMessageBusClient = typedMessageBusClient;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
			this.responseFormatter = responseFormatter;
		}


		public string UniqueName => BasycTypedMessageBusRequesterUniqueName;

		public void StartRequest(RequestResult requestResult)
		{
			var requestStartedSegment = requestResult.StartNewSegment("Requester started");
			var requestType = requestInfoTypeStorage.GetRequestType(requestResult.Request.RequestInfo);
			var paramValues = requestResult.Request.Parameters.Select(x => x.Value).ToArray();
			var requestObject = Activator.CreateInstance(requestType, paramValues);

			Task.Run(async () =>
			{
				var stopWatch = new Stopwatch();
				try
				{

					if (requestResult.Request.RequestInfo.HasResponse)
					{
						var waitingForBusSegment = requestStartedSegment.StartNewNestedSegment("Waiting for message bus");
						var response = await typedMessageBusClient.RequestAsync(requestType, requestObject, requestResult.Request.RequestInfo.ResponseType);
						waitingForBusSegment.End();
						if (response.Value is ErrorMessage error)
						{
							requestResult.Fail(error.Message);
						}
						else
						{
							var resultObject = response.AsT0;
							requestResult.Complete(responseFormatter.Format(resultObject));
						}
					}
					else
					{
						var waitingForBusSegment = requestStartedSegment.StartNewNestedSegment("Waiting for message bus");
						await typedMessageBusClient.SendAsync(requestType, requestObject)
						.ContinueWith(x =>
						{
							waitingForBusSegment.End();
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
								requestResult.Fail(errorMessage);
							}
							requestResult.Complete();

						});
					}
				}
				catch (Exception ex)
				{
					requestResult.Fail(ex.Message);
				}
			});
		}
	}
}