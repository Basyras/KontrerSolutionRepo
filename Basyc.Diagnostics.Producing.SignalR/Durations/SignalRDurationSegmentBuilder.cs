using Basyc.Diagnostics.Shared.Durations;

namespace Basyc.Diagnostics.Producing.SignalR.Durations
{
	public class SignalRDurationSegmentBuilder : DurationSegmentBuilderBase, IDurationSegmentBuilder
	{
		public SignalRDurationSegmentBuilder(string name) : base(name)
		{
			Name = name;
		}

		public override DateTimeOffset Start()
		{
			var startTime = base.Start();
			return startTime;

		}

		public override void End(DateTimeOffset finalEndTime)
		{
			throw new NotImplementedException();
		}

		public override IDurationSegmentBuilder EndAndStartNewFollowingSegment(string segmentName)
		{
			throw new NotImplementedException();
		}

		public override IDurationSegmentBuilder StartNewNestedSegment(string segmentName, DateTimeOffset start)
		{
			throw new NotImplementedException();
		}

		public override IDurationSegmentBuilder StartNewNestedSegment(string segmentName)
		{
			throw new NotImplementedException();
		}
	}
}
