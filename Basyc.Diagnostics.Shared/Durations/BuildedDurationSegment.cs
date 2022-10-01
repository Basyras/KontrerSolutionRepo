namespace Basyc.Diagnostics.Shared.Durations
{
	public record BuildedDurationSegment(ServiceIdentity Service, string Name, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan Duration, BuildedDurationSegment[] NestedSegments);
}
