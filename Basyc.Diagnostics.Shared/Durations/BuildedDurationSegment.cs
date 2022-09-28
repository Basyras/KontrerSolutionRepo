namespace Basyc.Diagnostics.Shared.Durations
{
	public record BuildedDurationSegment(string Name, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan Duration, BuildedDurationSegment[] NestedSegments);
}
