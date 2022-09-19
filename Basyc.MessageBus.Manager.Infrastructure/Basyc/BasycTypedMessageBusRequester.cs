using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Requesting;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using Basyc.MessageBus.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
	public class BasycTypedMessageBusRequester : IRequester
	{
		public const string BasycTypedMessageBusRequesterUniqueName = nameof(BasycTypedMessageBusRequester);


		private readonly IRequestInfoTypeStorage requestInfoTypeStorage;
		private readonly IResponseFormatter responseFormatter;
		private readonly IResultLoggingManager resultLoggingManager;
		private readonly ITypedMessageBusClient typedMessageBusClient;

		public BasycTypedMessageBusRequester(ITypedMessageBusClient typedMessageBusClient,
			IRequestInfoTypeStorage requestInfoTypeStorage,
			IResponseFormatter responseFormatter,
			IResultLoggingManager resultLoggingManager)
		{
			this.typedMessageBusClient = typedMessageBusClient;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
			this.responseFormatter = responseFormatter;
			this.resultLoggingManager = resultLoggingManager;
		}


		public string UniqueName => BasycTypedMessageBusRequesterUniqueName;


		public void StartRequest(RequestResult requestResult)
		{
			var requestStartedSegment = requestResult.StartNewSegment("Requester started");
			var requestType = requestInfoTypeStorage.GetRequestType(requestResult.Request.RequestInfo);
			var paramValues = requestResult.Request.Parameters.Select(x => x.Value).ToArray();
			var requestObject = Activator.CreateInstance(requestType, paramValues);

			try
			{
				if (requestResult.Request.RequestInfo.HasResponse)
				{
					var busTask = typedMessageBusClient.RequestAsync(requestType, requestObject, requestResult.Request.RequestInfo.ResponseType);
					requestResult.SessionId = busTask.SessionId;

					var waitingForBusSegment = requestStartedSegment.StartNewNestedSegment("Waiting for message bus");
					Task.Run(async () =>
					{
						var response = await busTask.Task;
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
					});
				}
				else
				{
					var waitingForBusSegment = requestStartedSegment.StartNewNestedSegment("Waiting for message bus");
					Task.Run(async () =>
					{
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

					});
				}
			}
			catch (Exception ex)
			{
				requestResult.Fail(ex.Message);
			}
		}
	}
}