using Basyc.Diagnostics.Server.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace Basyc.Diagnostics.SignalR.Server
{
	public class LoggingReceiversHub : Hub<ILoggingReceiversMethods>
	{
		private readonly IDiagnosticsServer diagnosticsServer;

		public LoggingReceiversHub(IDiagnosticsServer diagnosticsServer)
		{
			this.diagnosticsServer = diagnosticsServer;
		}
	}
}
