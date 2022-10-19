using Basyc.MessageBus.Client;
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
		private readonly IRequestDiagnosticsManager resultLoggingManager;
		private readonly BusManagerBasycDiagnosticsReceiverTraceIDMapper inMemorySessionMapper;
		private readonly ITypedMessageBusClient typedMessageBusClient;

		public BasycTypedMessageBusRequester(ITypedMessageBusClient typedMessageBusClient,
			IRequestInfoTypeStorage requestInfoTypeStorage,
			IResponseFormatter responseFormatter,
			IRequestDiagnosticsManager resultLoggingManager,
			BusManagerBasycDiagnosticsReceiverTraceIDMapper inMemorySessionMapper)
		{
			this.typedMessageBusClient = typedMessageBusClient;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
			this.responseFormatter = responseFormatter;
			this.resultLoggingManager = resultLoggingManager;
			this.inMemorySessionMapper = inMemorySessionMapper;
		}


		public string UniqueName => BasycTypedMessageBusRequesterUniqueName;


		public void StartRequest(Application.RequestContext requestContext, ILogger requestLogger)
		{
			var startSegment = requestContext.StartNewSegment("Requester started");
			var busRequestContext = new Shared.RequestContext(startSegment.Id, startSegment.TraceId);
			var prepareSegment = startSegment.StartNested("Preperaing request object");

			var requestType = requestInfoTypeStorage.GetRequestType(requestContext.Request.RequestInfo);
			var paramValues = requestContext.Request.Parameters.Select(x => x.Value).ToArray();
			var requestObject = Activator.CreateInstance(requestType, paramValues);
			prepareSegment.End();


			if (requestContext.Request.RequestInfo.HasResponse)
			{
				var busRequestActivity = startSegment.StartNested("Bus Request");

				using var requestStartActivity = busRequestActivity.StartNested("Request Start");
				var busTask = typedMessageBusClient.RequestAsync(requestType, requestObject, requestContext.Request.RequestInfo.ResponseType, busRequestContext);
				requestStartActivity.End();

				inMemorySessionMapper.AddMapping(requestContext.TraceId, busTask.TraceId);
				busTask.Task.ContinueWith(x =>
				{
					var endTime = DateTimeOffset.UtcNow;
					busRequestActivity.End(endTime);
					startSegment.End(endTime);

					if (x.IsFaulted)
					{
						requestContext.Fail(x.Exception.ToString());
						requestLogger.LogError($"Request handeling failed with exception: {x.Exception.ToString()}");
					}

					if (x.IsCanceled)
					{
						requestContext.Fail("canceled");
						requestLogger.LogError($"Request handeling was canceled");
					}

					if (x.IsCompletedSuccessfully)
					{
						if (x.Result.Value is ErrorMessage error)
						{
							requestContext.Fail(error.Message);
							requestLogger.LogError($"Request handler returned error. {error.Message}");
						}
						else
						{
							var resultObject = x.Result.AsT0;
							requestContext.Complete(responseFormatter.Format(resultObject));
							requestLogger.LogInformation($"Request completed");
						}
					}
				});

			}
			else
			{

				var busStartSegment = startSegment.StartNested("Requesting to bus");
				var busTask = typedMessageBusClient.SendAsync(requestType, requestObject, requestContext: busRequestContext);
				inMemorySessionMapper.AddMapping(requestContext.TraceId, busTask.TraceId);

				busTask.Task.ContinueWith(x =>
				{
					var endTime = DateTimeOffset.UtcNow;
					busStartSegment.End(endTime);
					startSegment.End(endTime);

					if (x.IsFaulted)
					{
						requestContext.Fail(x.Exception.ToString());
						requestLogger.LogError($"Request handeling failed with exception: {x.Exception.ToString()}");
					}

					if (x.IsCanceled)
					{
						requestContext.Fail("canceled");
						requestLogger.LogError($"Request handeling was canceled");
					}

					if (x.IsCompletedSuccessfully)
					{
						if (x.Result.Value is ErrorMessage error)
						{
							requestContext.Fail(error.Message);
							requestLogger.LogError($"Request handler returned error. {error.Message}");
						}
						else
						{
							requestContext.Complete();
							requestLogger.LogInformation($"Request completed");
						}
					}

				});
			}
		}
	}
}