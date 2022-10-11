using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.SignalR.Shared;
using Basyc.Diagnostics.SignalR.Shared.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class LoggingProducersHub : Hub<IProducersMethodsServerCanCall>, IServerMethodsProducersCanCall
	{
		private readonly IDiagnosticsServer diagnosticsServer;
		public LoggingProducersHub(IDiagnosticsServer diagnosticsServer)
		{
			this.diagnosticsServer = diagnosticsServer;
		}

		public Task ReceiveChangesFromProducer(ChangesSignalRDTO changesDTO)
		{
			return diagnosticsServer.ReceiveChanges(ChangesSignalRDTO.FromDto(changesDTO));
		}
	}
}
