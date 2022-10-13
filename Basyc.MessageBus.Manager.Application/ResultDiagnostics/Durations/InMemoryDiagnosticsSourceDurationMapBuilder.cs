using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Helpers;
using Basyc.Diagnostics.Shared.Logging;
using System;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics.Durations
{
	internal class InMemoryDiagnosticsSourceDurationMapBuilder : InMemoryDiagnosticsSourceDurationSegmentBuilder, IDurationMapBuilder
	{
		private readonly InMemoryRequestDiagnosticsSource diagnosticsSource;

		public InMemoryDiagnosticsSourceDurationMapBuilder(ServiceIdentity service, string traceId, string name, InMemoryRequestDiagnosticsSource diagnosticsSource) : base(service, traceId, Guid.NewGuid().ToString(), name, diagnosticsSource)
		{
			this.diagnosticsSource = diagnosticsSource;
		}


		public IDurationSegmentBuilder StartNewSegment(ServiceIdentity service, string segmentName, DateTimeOffset startTime)
		{
			return this.StartNested(service, segmentName, startTime);
		}

		public IDurationSegmentBuilder StartNewSegment(string segmentName)
		{
			//return ((IDurationSegmentBuilder)this).Start(segmentName);
			return this.StartNested(segmentName);
		}

		public IDurationSegmentBuilder StartNewSegment(string segmentName, DateTimeOffset startTime)
		{
			return this.StartNested(segmentName, startTime);
		}

		void IDurationMapBuilder.End()
		{
			//Root element is not really existing so it does not required to notify anyone
			//this.End();
			EndTime = DateTimeOffset.UtcNow;

		}

		public override IDurationSegmentBuilder StartNested(ServiceIdentity service, string segmentName, DateTimeOffset start)
		{
			var nestedId = IdGeneratorHelper.GenerateNewSpanId();
			diagnosticsSource.StartActivity(new ActivityStart(service, TraceId, null, nestedId, segmentName, start));
			return new InMemoryDiagnosticsSourceDurationSegmentBuilder(this, TraceId, nestedId, segmentName, service, diagnosticsSource);
		}
	}
}
