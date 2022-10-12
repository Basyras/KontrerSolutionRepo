using Basyc.Diagnostics.Shared.Logging;
using System.Diagnostics;

namespace Basyc.Diagnostics.Producing.Shared
{
	public struct ActivityDisposer : IDisposable
	{
		private readonly IDiagnosticsProducer diagnosticsProducer;
		private readonly bool isDisposed = false;
		public ActivityStart ActivityStart { get; init; }

		public ActivityDisposer(IDiagnosticsProducer diagnosticsProducer, ActivityStart activityStart)
		{
			this.diagnosticsProducer = diagnosticsProducer;
			this.ActivityStart = activityStart;
		}


		public void Dispose()
		{
			if (isDisposed is false)
				diagnosticsProducer.EndActivity(ActivityStart, DateTimeOffset.UtcNow);
		}

		public void End(DateTimeOffset endTime = default, ActivityStatusCode activityStatusCode = ActivityStatusCode.Ok)
		{
			if (endTime == default)
				endTime = DateTimeOffset.UtcNow;
			diagnosticsProducer.EndActivity(ActivityStart, endTime, activityStatusCode);
		}
	}
}
