using Basyc.Extensions.SignalR.Client;
using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Client.SignalR.Sessions;
using Basyc.MessageBus.HttpProxy.Shared.SignalR;
using Basyc.MessageBus.Shared;
using Basyc.Serialization.Abstraction;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using OneOf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Client.Http
{
	public class SignalRProxyObjectMessageBusClient : IObjectMessageBusClient
	{
		private readonly IStrongTypedHubConnectionPusherAndReceiver<IMethodsClientCanCall, IClientMethodsServerCanCall> hubConnection;
		private readonly IObjectToByteSerailizer byteSerializer;
		private readonly SignalRSessionManager sessionManager;

		public SignalRProxyObjectMessageBusClient(IOptions<SignalROptions> options, IObjectToByteSerailizer byteSerializer)
		{
			sessionManager = new SignalRSessionManager();


			hubConnection = new HubConnectionBuilder()
			.WithUrl(options.Value.SignalRServerUri + options.Value.ProxyClientHubPattern)
			.WithAutomaticReconnect()
			.BuildStrongTyped<IMethodsClientCanCall, IClientMethodsServerCanCall>(sessionManager);
			this.byteSerializer = byteSerializer;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await sessionManager.Start();
			await hubConnection.StartAsync();
		}

		public void Dispose()
		{
			hubConnection.DisposeAsync().GetAwaiter().GetResult();
		}

		public BusTask PublishAsync(string eventType, CancellationToken cancellationToken = default)
		{
			return CreateAndStartBusTask(eventType, null, cancellationToken).ToBusTask();

		}

		public BusTask PublishAsync(string eventType, object eventData, CancellationToken cancellationToken = default)
		{
			return CreateAndStartBusTask(eventType, eventData, cancellationToken).ToBusTask();
		}

		public BusTask<object> RequestAsync(string requestType, CancellationToken cancellationToken = default)
		{
			return BusTask<object>.FromBusTask(CreateAndStartBusTask(requestType, null, cancellationToken), x => x!);

		}

		public BusTask<object> RequestAsync(string requestType, object requestData, CancellationToken cancellationToken = default)
		{
			return BusTask<object>.FromBusTask(CreateAndStartBusTask(requestType, requestData, cancellationToken), x => x!);
		}

		public BusTask SendAsync(string commandType, CancellationToken cancellationToken = default)
		{
			return CreateAndStartBusTask(commandType, null, cancellationToken).ToBusTask();
		}

		public BusTask SendAsync(string commandType, object commandData, CancellationToken cancellationToken = default)
		{
			return CreateAndStartBusTask(commandType, commandData, cancellationToken).ToBusTask();
		}

		private BusTask<object?> CreateAndStartBusTask(string requestType, object? requestData = null, CancellationToken cancellationToken = default)
		{
			SignalRSession session = sessionManager.StartSession();
			Task<OneOf<object?, ErrorMessage>> reqeustTask = Task.Run(async () =>
			{
				if (byteSerializer.TrySerialize(requestData, requestType, out var requestDataBytes, out var error) is false)
				{
					return new ErrorMessage("Failed to serailize request");
				}
				try
				{
					await hubConnection.Call.Request(new RequestSignalRDTO(requestType, true, requestDataBytes));
				}
				catch (Exception ex)
				{
					return new ErrorMessage("Failed while requesting. " + ex.Message);
				}

				var result = await session.WaitForCompletion();
				return result.Match<OneOf<object?, ErrorMessage>>(resultDTO =>
				{
					if (resultDTO.HasResponse)
					{
						if (byteSerializer.TryDeserialize(resultDTO.ResponseData!, resultDTO.ResponseType!, out var deserializedResult, out var error) is false)
						{
							return new ErrorMessage("Failed to serailize response");
						}
						return deserializedResult;

					}
					else
					{
						return (OneOf<object?, ErrorMessage>)null;
					}
				},
				error => error);

			});

			return BusTask<object?>.FromTask(session.SessionId, reqeustTask);

		}
	}
}