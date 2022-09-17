using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.Server.Abstractions.Building;
using Basyc.Diagnostics.SignalR.Server;

namespace Microsoft.Extensions.DependencyInjection;

public static class SelectDiagnosticsServerStageSignalRExtensions
{
	/// <summary>
	/// Call <see cref="WepApplicationDiagnosticsSignalRExtensions.MapBasycSignalRDiagnosticsServer(WebApplication, string)"/> to start 
	/// </summary>
	/// <param name="parent"></param>
	/// <returns></returns>
	public static void UseSignalR(this SelectDiagnosticsServerStage parent)
	{
		parent.services.AddSignalR();
		parent.services.AddSingleton<IDiagnosticsServer, SignalRDiagnosticsServer>();
	}
}
