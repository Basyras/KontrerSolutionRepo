﻿namespace Basyc.Diagnostics.Shared.Durations
{
	public class DurationMapBuilder : IDurationMapBuilder
	{
		public DurationMapBuilder(ServiceIdentity service)
		{
			Service = service;
		}

		private InMemoryDurationSegmentBuilder? rootSegmentBuilder;
		public DateTimeOffset StartTime { get; private set; }
		public DateTimeOffset EndTime { get; private set; }
		public bool HasEnded => EndTime != default;

		public bool HasStarted { get; private set; }
		public ServiceIdentity Service { get; }

		/// <summary>
		/// Return start time
		/// </summary>
		/// <returns></returns>
		public DateTimeOffset Start()
		{
			rootSegmentBuilder = new InMemoryDurationSegmentBuilder("root", Service, () => rootSegmentBuilder!);
			StartTime = rootSegmentBuilder.Start();
			HasStarted = true;
			return StartTime;
		}

		public IDurationSegmentBuilder StartNewSegment(string segmentName)
		{
			if (HasStarted is false)
			{
				var mapStart = Start();
				var newSegment = rootSegmentBuilder!.StartNested(segmentName, mapStart);
				return newSegment;
			}
			else
			{
				var newSegment = rootSegmentBuilder!.StartNested(segmentName);
				return newSegment;
			}
		}

		public IDurationSegmentBuilder StartNewSegment(ServiceIdentity service, string segmentName, DateTimeOffset startTime)
		{
			DateTimeOffset mapStart = HasStarted is false ? Start() : StartTime;

			if (mapStart > startTime)
				throw new ArgumentException("nested segment cant start before map starts");

			var newSegment = rootSegmentBuilder!.StartNested(service, segmentName, startTime);
			return newSegment;
		}


		public IDurationSegmentBuilder StartNewSegment(string segmentName, DateTimeOffset startTime)
		{
			return StartNewSegment(Service, segmentName, startTime);
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
				if (HasEnded is false)
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
