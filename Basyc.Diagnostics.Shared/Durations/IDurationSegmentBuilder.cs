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

		IDurationSegmentBuilder StartNewNestedSegment(string segmentName);
		IDurationSegmentBuilder StartNewNestedSegment(string segmentName, DateTimeOffset start);
		IDurationSegmentBuilder EndAndStartNewFollowingSegment(string segmentName);

		void Dispose();
	}
}