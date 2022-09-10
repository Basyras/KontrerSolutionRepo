using System;

namespace Basyc.MessageBus.Manager.Application.Durations
{
	public record DurationSegment(string Name, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan Duration, DurationSegment[] NestedSegments);
}
