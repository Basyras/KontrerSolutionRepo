using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Basyc.Extensions.SignalR.Client.Tests.Mocks
{
	public class HubConnectionMock : HubConnection
	{
		public HubConnectionMock(IConnectionFactory connectionFactory,
			IHubProtocol protocol,
			EndPoint endPoint,
			IServiceProvider serviceProvider,
			ILoggerFactory loggerFactory,
			IRetryPolicy reconnectPolicy)
			: base(connectionFactory, protocol, endPoint, serviceProvider, loggerFactory, reconnectPolicy)
		{

		}

		public override Task SendCoreAsync(string methodName, object?[] args, CancellationToken cancellationToken = default)
		{
			OnSendingCore(new(methodName, args, cancellationToken));
			return Task.CompletedTask;
		}

		public new Task StartAsync(CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}

		public event EventHandler<SendingCoreArgs>? SendingCore;
		private void OnSendingCore(SendingCoreArgs args)
		{
			LastSendCoreCall = args;
			SendingCore?.Invoke(this, args);
		}

		public SendingCoreArgs? LastSendCoreCall { get; private set; }

	}
}
