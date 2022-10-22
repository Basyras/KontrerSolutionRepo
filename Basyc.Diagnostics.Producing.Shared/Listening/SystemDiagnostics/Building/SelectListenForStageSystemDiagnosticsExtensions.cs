using Basyc.Diagnostics.Producing.Shared.Listening;
using Basyc.Diagnostics.Producing.Shared.Listening.Building;
using Basyc.Diagnostics.Producing.Shared.Listening.SystemDiagnostics;
using System.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class SelectListenForStageSystemDiagnosticsExtensions
	{
		public static void AnyActvity(this SelectListenForStage parent)
		{
			parent.services.AddSingleton<IListener, SystemDiagnosticsListener>();
		}

		public static void Actvity(this SelectListenForStage parent, Func<Activity, bool> filter)
		{
			parent.services.Configure<SystemDiagnosticsListenerOptions>(x => { x.Filter = filter; });
			parent.services.AddSingleton<IListener, SystemDiagnosticsListener>();

		}
	}
}
