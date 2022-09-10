using System;

namespace Basyc.MessageBus.Manager.Application.Durations
{
	public class DurationMapBuilder
	{
		private DurationSegmentBuilder? rootSegmentBuilder;
		public DateTimeOffset StartTime { get; private set; }
		public DateTimeOffset EndTime { get; private set; }
		public bool MapFinished => EndTime != default;

		public bool HasStartedCounting { get; private set; }

		/// <summary>
		/// Return start time
		/// </summary>
		/// <returns></returns>
		public DateTimeOffset Start()
		{
			StartTime = DateTimeOffset.UtcNow;
			rootSegmentBuilder = new DurationSegmentBuilder("root", StartTime);
			HasStartedCounting = true;
			return StartTime;
		}

		public DurationSegmentBuilder StartNewSegment(string segmentName)
		{
			if (HasStartedCounting is false)
			{
				var mapStart = Start();
				var newSegment = rootSegmentBuilder!.StartNewNestedSegment(segmentName, mapStart);
				return newSegment;
			}
			else
			{
				var newSegment = rootSegmentBuilder!.StartNewNestedSegment(segmentName);
				return newSegment;
			}

		}

		public void End()
		{
			End(DateTimeOffset.UtcNow);
		}

		private void End(DateTimeOffset endTime)
		{
			if (HasStartedCounting is false)
				throw new InvalidOperationException($"{nameof(End)} method must be called after {nameof(Start)} or other method that {nameof(Start)} calls internally ({nameof(Build)})");
			EndTime = DateTimeOffset.UtcNow;
		}


		public DurationMap Build()
		{
			if (HasStartedCounting)
			{
				if (MapFinished is false)
				{
					End();
				}
			}
			else
			{
				var startTime = Start();
				End(startTime);
			}

			var rootSegment = rootSegmentBuilder!.Build(EndTime);
			var totalDuration = EndTime - StartTime;
			return new DurationMap(rootSegment.NestedSegments, totalDuration, StartTime, EndTime);
		}

	}
}
