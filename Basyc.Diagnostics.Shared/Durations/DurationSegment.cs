namespace Basyc.Diagnostics.Shared.Durations
{
	public record DurationSegment(string Name, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan Duration, DurationSegment[] NestedSegments);
}
