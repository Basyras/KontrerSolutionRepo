using Basyc.Diagnostics.Shared.Durations;
using Basyc.Diagnostics.Shared.Logging;
using System;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics.Durations
{
	internal class InMemoryDiagnosticsSourceDurationSegmentBuilder : DurationSegmentBuilderBase
	{
		public string TraceId { get; init; }
		private readonly string id;
		private readonly InMemoryRequestDiagnosticsSource diagnosticsSource;
		private readonly InMemoryDiagnosticsSourceDurationSegmentBuilder? parent;

		public InMemoryDiagnosticsSourceDurationSegmentBuilder(ServiceIdentity service, string traceId, string name, InMemoryRequestDiagnosticsSource diagnosticsSource) : base(name, service)
		{
			this.TraceId = traceId;
			this.id = Guid.NewGuid().ToString();
			this.diagnosticsSource = diagnosticsSource;
		}

		public InMemoryDiagnosticsSourceDurationSegmentBuilder(InMemoryDiagnosticsSourceDurationSegmentBuilder parent, string traceId, string id, string name, ServiceIdentity service, InMemoryRequestDiagnosticsSource diagnosticsSource) : base(name, service)
		{
			this.TraceId = traceId;
			this.id = id;
			this.diagnosticsSource = diagnosticsSource;
			this.parent = parent;
			HasParent = true;
		}

		public override void End(DateTimeOffset finalEndTime)
		{
			EndTime = finalEndTime;
			diagnosticsSource.EndActivity(new ActivityEnd(Service, TraceId, parent?.id, id, Name, StartTime, EndTime, System.Diagnostics.ActivityStatusCode.Ok));
		}

		public override IDurationSegmentBuilder EndAndStartFollowing(string segmentName)
		{
			if (HasParent is false)
				throw new InvalidOperationException("Segment must have a parent in order to start following segment");

			var endTime = End();
			return parent!.StartNested(segmentName, endTime);
		}

		public override IDurationSegmentBuilder StartNested(ServiceIdentity service, string segmentName, DateTimeOffset start)
		{
			var nestedId = Guid.NewGuid().ToString();
			diagnosticsSource.StartActivity(new ActivityStart(service, TraceId, id, nestedId, segmentName, start));
			return new InMemoryDiagnosticsSourceDurationSegmentBuilder(this, TraceId, nestedId, segmentName, service, diagnosticsSource);
		}
	}
}
