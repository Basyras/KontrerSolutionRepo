using Basyc.Diagnostics.Shared.Durations;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics.Durations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public class RequestManager : IRequestManager
	{
		public static readonly ActivitySource RequestManagerRequestActivity = new ActivitySource("RequestManager.StartRequest", "1.0.0");


		private readonly IRequesterSelector requesterSelector;
		private readonly IRequestDiagnosticsManager requestDiagnosticsManager;
		private readonly InMemoryRequestDiagnosticsSource inMemoryRequestDiagnosticsSource;
		private int requestCounter;
		private readonly ServiceIdentity requestManagerServiceIdentity = new ServiceIdentity("RequestManager");

		public RequestManager(IRequesterSelector requesterSelector, IRequestDiagnosticsManager loggingManager, InMemoryRequestDiagnosticsSource inMemoryRequestDiagnosticsSource)
		{
			this.requesterSelector = requesterSelector;
			this.requestDiagnosticsManager = loggingManager;
			this.inMemoryRequestDiagnosticsSource = inMemoryRequestDiagnosticsSource;
		}

		public Dictionary<RequestInfo, List<RequestContext>> Results { get; } = new Dictionary<RequestInfo, List<RequestContext>>();


		public RequestContext StartRequest(Request request)
		{
			var traceId = Interlocked.Increment(ref requestCounter).ToString();

			//BusHandlerLoggerSessionManager.StartSession(new LoggingSession(traceId, handlerMetadata.HandlerInfo.HandleMethodInfo.Name));

			var normilizedTraceId = traceId.PadLeft(32, '0');
			//var activityTraceId = ActivityTraceId.CreateFromString(normilizedTraceId);
			//var activitySpanId = ActivitySpanId.CreateFromString(parentSpanId);
			//var activityContext = new System.Diagnostics.ActivityContext(activityTraceId, default, ActivityTraceFlags.Recorded, null, false);
			using (var handlerStartedActivity = RequestManagerRequestActivity.StartActivity("RequestManager.StartRequest", ActivityKind.Internal, normilizedTraceId))
			{
				if (Results.TryGetValue(request.RequestInfo, out var reqeustContexts) is false)
				{
					reqeustContexts = new List<RequestContext>();
					Results.Add(request.RequestInfo, reqeustContexts);
				}

				var requester = requesterSelector.PickRequester(request.RequestInfo);
				RequestDiagnosticContext requestDiagnostics = requestDiagnosticsManager.CreateDiagnostics(traceId);
				IDurationMapBuilder durationMapBuilder = new InMemoryDiagnosticsSourceDurationMapBuilder(requestManagerServiceIdentity, traceId, "root", inMemoryRequestDiagnosticsSource);
				var requestContext = new RequestContext(request, DateTime.Now, traceId, durationMapBuilder, requestDiagnostics);
				reqeustContexts.Add(requestContext);
				requester.StartRequest(requestContext, new ResultLoggingContextLogger(requestManagerServiceIdentity, requestDiagnostics));
				requestDiagnostics.Log(requestManagerServiceIdentity, DateTimeOffset.UtcNow, LogLevel.Information, "Picking requester");
				requestDiagnostics.Log(requestManagerServiceIdentity, DateTimeOffset.UtcNow, LogLevel.Information, "Starting request");
				requestDiagnostics.Log(requestManagerServiceIdentity, DateTimeOffset.UtcNow, LogLevel.Information, "Request started");


				return requestContext;
			}
		}
	}
}
