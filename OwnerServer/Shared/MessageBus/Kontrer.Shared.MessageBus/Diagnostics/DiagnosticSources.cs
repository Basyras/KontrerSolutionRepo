using System.Diagnostics;

namespace Basyc.MessageBus.Client.Diagnostics
{
	public static class DiagnosticSources
	{
		public static readonly ActivitySource HandlerStarted = new ActivitySource("Basyc.Bus.HandlerStarted", "1.0.0");

	}
}
