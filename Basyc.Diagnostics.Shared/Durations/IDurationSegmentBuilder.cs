namespace Basyc.Diagnostics.Shared.Durations
{
	public interface IDurationSegmentBuilder
	{
		string Name { get; init; }

		bool HasStarted { get; }
		bool HasEnded { get; }
		DateTimeOffset StartTime { get; }
		DateTimeOffset EndTime { get; }

		DateTimeOffset Start();
		void Start(DateTimeOffset segmentStart);
		DateTimeOffset End();
		void End(DateTimeOffset finalEndTime);

		IDurationSegmentBuilder StartNewNestedSegment(ServiceIdentity service, string segmentName, DateTimeOffset start);
		IDurationSegmentBuilder StartNewNestedSegment(string segmentName, DateTimeOffset start);
		IDurationSegmentBuilder StartNewNestedSegment(ServiceIdentity service, string segmentName);
		IDurationSegmentBuilder StartNewNestedSegment(string segmentName);
		IDurationSegmentBuilder EndAndStartNewFollowingSegment(string segmentName);

		void Dispose();
	}
}