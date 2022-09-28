namespace Basyc.Diagnostics.Shared.Durations
{
	public class DurationMapBuilder
	{
		private InMemoryDurationSegmentBuilder? rootSegmentBuilder;
		public DateTimeOffset StartTime { get; private set; }
		public DateTimeOffset EndTime { get; private set; }
		public bool MapFinished => EndTime != default;

		public bool HasStarted { get; private set; }

		/// <summary>
		/// Return start time
		/// </summary>
		/// <returns></returns>
		public DateTimeOffset Start()
		{
			rootSegmentBuilder = new InMemoryDurationSegmentBuilder("root", () => rootSegmentBuilder!);
			StartTime = rootSegmentBuilder.Start();
			HasStarted = true;
			return StartTime;
		}

		public IDurationSegmentBuilder StartNewSegment(string segmentName)
		{
			if (HasStarted is false)
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
			if (HasStarted is false)
				throw new InvalidOperationException($"{nameof(End)} method must be called after {nameof(Start)} or other method that {nameof(Start)} calls internally ({nameof(Build)})");
			EndTime = DateTimeOffset.UtcNow;
		}


		public DurationMap Build()
		{
			if (HasStarted)
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
