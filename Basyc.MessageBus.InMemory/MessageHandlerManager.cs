using Basyc.MessageBus.Client.Diagnostics;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.MessageBus.NetMQ.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneOf;
using System.Diagnostics;

namespace Basyc.MessageBus.Client.NetMQ
{
	public class MessageHandlerManager : IMessageHandlerManager
	{
		private record HandlerMetadata(NetMQMessageHandlerInfo HandlerInfo, Type HandlerRuntimeType);

		private class DiagnosticListenerObserver : IObserver<KeyValuePair<string, object>>
		{
			private readonly Action<Activity> activityStarted;
			private readonly Action<Activity> activityStopped;

			public DiagnosticListenerObserver(Action<Activity> activityStarted, Action<Activity> activityStopped)
			{
				this.activityStarted = activityStarted;
				this.activityStopped = activityStopped;
			}
			public void OnCompleted()
			{
			}

			public void OnError(Exception error)
			{
			}

			public void OnNext(KeyValuePair<string, object> value)
			{
				var activity = (Activity)value.Value;
				if (value.Key.Contains("Start"))
				{
					activityStarted(activity);
				}
				else
				{
					activityStopped(activity);
				}
			}
		}

		private readonly IServiceProvider serviceProvider;
		private readonly IOptions<MessageHandlerManagerOptions> options;
		private readonly DiagnosticListener diagnosticListener;
		private readonly DiagnosticListenerObserver diagnosticListenerObserver;
		private readonly Dictionary<string, HandlerMetadata> handlerTypesCacheMap = new();
		public MessageHandlerManager(IServiceProvider serviceProvider, IOptions<MessageHandlerManagerOptions> options)
		{
			this.serviceProvider = serviceProvider;
			this.options = options;
			diagnosticListener = new DiagnosticListener("MessageHandlerManagerDiagnosticsListener");
			diagnosticListenerObserver = new DiagnosticListenerObserver(ActivityStarted, ActivityStopped);


			foreach (var handlerInfo in options.Value.HandlerInfos)
			{
				Type handlerInterfaceType;
				Type handlerType;

				if (handlerInfo.HasResponse)
				{
					handlerInterfaceType = typeof(IMessageHandler<,>);
					handlerType = handlerInterfaceType.MakeGenericType(handlerInfo.MessageType, handlerInfo.ResponseType!);

				}
				else
				{
					handlerInterfaceType = typeof(IMessageHandler<>);
					handlerType = handlerInterfaceType.MakeGenericType(handlerInfo.MessageType);
				}
				HandlerMetadata newHandlerMetadata = new(handlerInfo, handlerType);
				handlerTypesCacheMap.Add(handlerInfo.MessageSimpleType, newHandlerMetadata);
			}

		}

		public async Task<OneOf<object, Exception>> ConsumeMessage(string messageType, object? messageData, CancellationToken cancellationToken, string traceId)
		{
			if (handlerTypesCacheMap.TryGetValue(messageType, out var handlerMetadata) is false)
			{
				throw new InvalidOperationException("Handler for this message not found");
			}

			object handler = serviceProvider.GetRequiredService(handlerMetadata.HandlerRuntimeType)!;
			BusHandlerLoggerSessionManager.StartSession(new LoggingSession(traceId, handlerMetadata.HandlerInfo.HandleMethodInfo.Name));

			//var activityTraceId = ActivityTraceId.CreateFromString("1".PadRight(32, '0'));
			var activityTraceId = ActivityTraceId.CreateFromString(traceId.PadLeft(32, '0'));
			var activitySpanId = ActivitySpanId.CreateFromString("11".PadRight(16, '0'));
			var activityContext = new ActivityContext(activityTraceId, activitySpanId, ActivityTraceFlags.Recorded);

			var activity = new Activity("handler started");
			activity.SetParentId(activityTraceId, activitySpanId, ActivityTraceFlags.Recorded);
			diagnosticListener.StartActivity(activity, activity);
			//using (DiagnosticSources.HandlerStarted.StartActivity("handler started", ActivityKind.Client, activityContext))
			Task handlerResultTask = (Task)handlerMetadata.HandlerInfo.HandleMethodInfo.Invoke(handler, new object[] { messageData!, cancellationToken })!;
			object? handlerResult;
			try
			{
				if (handlerMetadata.HandlerInfo.HasResponse)
				{
					object taskResult = ((dynamic)handlerResultTask).Result!;
					handlerResult = taskResult;
				}
				else
				{
					await handlerResultTask;
					handlerResult = new VoidResult();
				}
			}
			catch (Exception ex)
			{
				BusHandlerLoggerSessionManager.EndSession();
				diagnosticListener.StopActivity(activity, activity);
				return ex;
			}
			BusHandlerLoggerSessionManager.EndSession();
			diagnosticListener.StopActivity(activity, activity);
			return handlerResult;

		}

		public string[] GetConsumableMessageTypes()
		{
			return handlerTypesCacheMap.Values
				.Select(handlerMetadata => handlerMetadata.HandlerInfo.MessageSimpleType)
				.ToArray();
		}

		private void ActivityStarted(Activity activity)
		{

		}

		private void ActivityStopped(Activity activity)
		{

		}


	}
}

