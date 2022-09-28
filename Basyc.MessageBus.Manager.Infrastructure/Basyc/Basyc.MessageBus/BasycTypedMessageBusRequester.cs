using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Requesting;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using Basyc.MessageBus.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Basyc.MessageBus.Manager.Infrastructure.Basyc.Basyc.MessageBus
{
	public class BasycTypedMessageBusRequester : IRequester
	{
		public const string BasycTypedMessageBusRequesterUniqueName = nameof(BasycTypedMessageBusRequester);


		private readonly IRequestInfoTypeStorage requestInfoTypeStorage;
		private readonly IResponseFormatter responseFormatter;
		private readonly IResultLoggingManager resultLoggingManager;
		private readonly BusManagerBasycDiagnosticsReceiverSessionMapper inMemorySessionMapper;
		private readonly ITypedMessageBusClient typedMessageBusClient;

		public BasycTypedMessageBusRequester(ITypedMessageBusClient typedMessageBusClient,
			IRequestInfoTypeStorage requestInfoTypeStorage,
			IResponseFormatter responseFormatter,
			IResultLoggingManager resultLoggingManager,
			BusManagerBasycDiagnosticsReceiverSessionMapper inMemorySessionMapper)
		{
			this.typedMessageBusClient = typedMessageBusClient;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
			this.responseFormatter = responseFormatter;
			this.resultLoggingManager = resultLoggingManager;
			this.inMemorySessionMapper = inMemorySessionMapper;
		}


		public string UniqueName => BasycTypedMessageBusRequesterUniqueName;


		public void StartRequest(RequestResult requestResult, ILogger requestLogger)
		{
			var requestStartedSegment = requestResult.StartNewSegment("Requester started");
			var requestType = requestInfoTypeStorage.GetRequestType(requestResult.Request.RequestInfo);
			var paramValues = requestResult.Request.Parameters.Select(x => x.Value).ToArray();
			var requestObject = Activator.CreateInstance(requestType, paramValues);

			if (requestResult.Request.RequestInfo.HasResponse)
			{
				var busTask = typedMessageBusClient.RequestAsync(requestType, requestObject, requestResult.Request.RequestInfo.ResponseType);
				inMemorySessionMapper.AddMapping(requestResult.SessionId, busTask.SessionId);
				var waitingForBusSegment = requestStartedSegment.StartNewNestedSegment("Waiting for message bus");
				busTask.Task.ContinueWith(x =>
				{
					waitingForBusSegment.End();
					if (x.IsFaulted)
						requestResult.Fail(x.Exception.ToString());

					if (x.IsCanceled)
						requestResult.Fail("canceled");

					if (x.IsCompletedSuccessfully)
					{
						if (x.Result.Value is ErrorMessage error)
						{
							requestResult.Fail(error.Message);
						}
						else
						{
							var resultObject = x.Result.AsT0;
							requestResult.Complete(responseFormatter.Format(resultObject));
						}
					}

					if (x.IsCanceled)
						requestResult.Fail("Unknown error");

				});

			}
			else
			{
				var busTask = typedMessageBusClient.SendAsync(requestType, requestObject);
				inMemorySessionMapper.AddMapping(requestResult.SessionId, busTask.SessionId);

				var waitingForBusSegment = requestStartedSegment.StartNewNestedSegment("Waiting for message bus");
				busTask.Task.ContinueWith(x =>
				{
					waitingForBusSegment.End();
					if (x.IsFaulted)
						requestResult.Fail(x.Exception.ToString());

					if (x.IsCanceled)
						requestResult.Fail("canceled");

					if (x.IsCompletedSuccessfully)
					{
						if (x.Result.Value is ErrorMessage error)
						{
							requestResult.Fail(error.Message);
						}
						else
						{
							requestResult.Complete();
						}
					}

					if (x.IsCanceled)
						requestResult.Fail("Unknown error");

				});
			}
		}
	}
}