using Basyc.MessageBus.Manager.Infrastructure.SignalR.ResultDiagnostics;

namespace Microsoft.AspNetCore.Builder
{
	public static class WepApplicationDiagnosticsSignalRExtensions
	{
		public static void MapSignalRLogSource(this WebApplication webApplication, string hubUriPattern = "/logHub")
		{
			webApplication.MapHub<LogHub>(hubUriPattern);
		}

	}
}
