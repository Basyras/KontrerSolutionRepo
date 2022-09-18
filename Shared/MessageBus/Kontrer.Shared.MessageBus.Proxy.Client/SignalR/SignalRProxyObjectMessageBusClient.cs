using Basyc.MessageBus.Client;
using Basyc.MessageBus.HttpProxy.Shared.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Basyc.MessageBus.HttpProxy.Client.Http
{
	public class SignalRProxyObjectMessageBusClient : IObjectMessageBusClient
	{
		private readonly HubConnection hubConnection;

		public SignalRProxyObjectMessageBusClient(IOptions<SignalROptions> options)
		{
			hubConnection = new HubConnectionBuilder()
			.WithUrl(options.Value.SignalRServerUri + options.Value.ProxyClientHubPattern)
			.WithAutomaticReconnect()
			.Build();

			hubConnection.On(SignalRConstants.ReceiveRequestResultMetadataMessage, (Action<LogEntrySignalRDTO[]>)(logEntriesDtos =>
			{
				LogEntry[] logEntries = logEntriesDtos.Select(x => LogEntrySignalRDTO.ToLogEntry(x)).ToArray();
				OnLogsReceived(logEntries);
			}));
		}

		public void Dispose()
		{

		}



		Task IObjectMessageBusClient.PublishAsync(string eventType, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(eventType, null, null, cancellationToken);
		}

		Task IObjectMessageBusClient.PublishAsync(string eventType, object eventData, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(eventType, eventData, null, cancellationToken);
		}

		Task IObjectMessageBusClient.SendAsync(string commandType, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(commandType, null, null, cancellationToken);
		}

		Task IObjectMessageBusClient.SendAsync(string commandType, object commandData, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(commandType, commandData, null, cancellationToken);
		}

		Task<object> IObjectMessageBusClient.RequestAsync(string requestType, CancellationToken cancellationToken)
		{
			return HttpCallToProxyServer(requestType, null, typeof(UknownResponseType), cancellationToken);
		}

		BusTask<object> IObjectMessageBusClient.RequestAsync(string requestType, object requestData, CancellationToken cancellationToken)
		{
			var proxyCallTask = HttpCallToProxyServer(requestType, requestData, typeof(UknownResponseType), cancellationToken);
			var busTask = BusTask<object>.FromTask(-1, proxyCallTask);
			return busTask;
		}

		Task IObjectMessageBusClient.StartAsync(CancellationToken cancellationToken)
		{
			var hubConnection = HubConnectionBuilder
		}

		void IDisposable.Dispose()
		{

		}

		private class UknownResponseType { };
	}
}