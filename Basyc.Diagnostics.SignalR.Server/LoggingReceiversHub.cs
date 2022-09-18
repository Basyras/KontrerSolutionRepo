using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.SignalR.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class LoggingReceiversHub : Hub<IReceiversMethodsServerCanCall>, IServerMethodsReceiversCanCall
	{
		private readonly IDiagnosticsServer diagnosticsServer;

		public LoggingReceiversHub(IDiagnosticsServer diagnosticsServer)
		{
			this.diagnosticsServer = diagnosticsServer;
		}
	}
}
