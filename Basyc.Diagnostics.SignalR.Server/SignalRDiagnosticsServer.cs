using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.Shared.Logging;
using Basyc.Diagnostics.SignalR.Shared;
using Basyc.Diagnostics.SignalR.Shared.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class SignalRDiagnosticsServer : IDiagnosticsServer
	{
		private readonly IHubContext<LoggingReceiversHub, IReceiversMethodsServerCanCall> receiversHubContext;

		public SignalRDiagnosticsServer(IHubContext<LoggingReceiversHub, IReceiversMethodsServerCanCall> receiversHubContext)
		{
			this.receiversHubContext = receiversHubContext;
		}

		public Task ReceiveChanges(DiagnosticChange change)
		{
			var changeDTO = ChangesSignalRDTO.ToDto(change);
			return receiversHubContext.Clients.All.ReceiveChangesFromServer(changeDTO);
		}
	}
}
