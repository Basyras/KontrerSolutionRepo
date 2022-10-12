using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.SignalR.Shared;
using Basyc.Diagnostics.SignalR.Shared.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class LoggingProducersHub : Hub<IProducersMethodsServerCanCall>, IServerMethodsProducersCanCall
	{
		private readonly InMemoryServerDiagnosticReceiver diagnosticsServer;
		public LoggingProducersHub(InMemoryServerDiagnosticReceiver diagnosticsServer)
		{
			this.diagnosticsServer = diagnosticsServer;
		}

		public Task ReceiveChangesFromProducer(ChangesSignalRDTO changesDTO)
		{
			diagnosticsServer.ReceiveChangesFromProducer(ChangesSignalRDTO.FromDto(changesDTO));
			return Task.CompletedTask;
		}
	}
}
