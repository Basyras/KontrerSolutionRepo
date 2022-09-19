using Basyc.MessageBus.HttpProxy.Shared.SignalR;
using Basyc.MessageBus.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Client.SignalR.Sessions
{
	public class SignalRSessionManager : IClientMethodsServerCanCall
	{
		private readonly Channel<object> clientServerChannel = Channel.CreateUnbounded<object>();
		private readonly Dictionary<int, SignalRSession> sessionMap = new();
		private int sessionCounter = 0;

		public Task ReceiveRequestFailed(RequestFailedSignalRDTO requestFailed)
		{
			return clientServerChannel.Writer.WriteAsync(requestFailed).AsTask();
		}

		public Task ReceiveRequestResult(ResponseSignalRDTO response)
		{
			return clientServerChannel.Writer.WriteAsync(response).AsTask();
		}

		public Task ReceiveRequestResultMetadata(RequestMetadataSignalRDTO requestMetadata)
		{
			//Not used for now
			return Task.CompletedTask;
		}

		public Task Start()
		{
			Task.Run(async () =>
			{
				await foreach (var responseObject in clientServerChannel.Reader.ReadAllAsync())
				{
					SignalRSession session = default;

					switch (responseObject)
					{
						case ResponseSignalRDTO response:
							session = sessionMap[response.SessionId];
							session.Complete(response);
							sessionMap.Remove(response.SessionId);
							break;
						case RequestFailedSignalRDTO error:
							session = sessionMap[error.SessionId];
							session.Complete(new ErrorMessage(error.Message));
							sessionMap.Remove(error.SessionId);
							break;
						default:
							throw new ArgumentException("message not recognized");
					}
				}
			});

			return Task.CompletedTask;
		}

		public SignalRSession StartSession()
		{
			var sessionIndex = Interlocked.Increment(ref sessionCounter);
			var session = new SignalRSession(sessionIndex);
			sessionMap.Add(sessionIndex, session);
			return session;
		}
	}
}
