using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.Durations
{
	public class DurationSegmentBuilder : IDisposable
	{
		private readonly List<DurationSegmentBuilder> nestedSegmentBuilders = new List<DurationSegmentBuilder>();
		public DateTimeOffset StartTime { get; init; }
		private DateTimeOffset endTime;
		public bool SegmentFinished { get; private set; }

		public DurationSegmentBuilder(string name, DateTimeOffset segmentStart)
		{
			Name = name;
			StartTime = segmentStart;
		}

		public string Name { get; init; }

		/// <summary>
		/// Ensures that <see cref="End"/> was called and produce <see cref="DurationSegment"/> with processed timestamps
		/// </summary>
		public DurationSegment Build()
		{
			if (SegmentFinished is false)
			{
				End();
			}

			return Build(endTime);
		}

		/// <summary>
		/// Ensures that <see cref="End"/> was called and produce <see cref="DurationSegment"/> with processed timestamps
		/// </summary>
		public DurationSegment Build(DateTimeOffset endTimeToUseWhenNotYetEnded)
		{
			if (SegmentFinished is false)
			{
				End(endTimeToUseWhenNotYetEnded);
			}

			return BuildNestedSegments();
		}

		private DurationSegment BuildNestedSegments()
		{
			DurationSegment[] nestedSegments = new DurationSegment[nestedSegmentBuilders.Count];
			for (int nestedSegmentIndex = 0; nestedSegmentIndex < nestedSegmentBuilders.Count; nestedSegmentIndex++)
			{
				DurationSegmentBuilder? nestedSegmentBuilder = nestedSegmentBuilders[nestedSegmentIndex];
				var nestedSegment = nestedSegmentBuilder.Build(endTime);
				nestedSegments[nestedSegmentIndex] = nestedSegment;
			}
			return new DurationSegment(Name, StartTime, endTime, endTime - StartTime, nestedSegments);
		}

		public DateTimeOffset End()
		{
			End(DateTimeOffset.UtcNow);
			return endTime;
		}

		private void End(DateTimeOffset parentEnd)
		{
			if (SegmentFinished)
			{
				throw new InvalidOperationException($"{nameof(End)} was called twice");
			}

			endTime = parentEnd;

			foreach (var nestedSegment in nestedSegmentBuilders)
			{
				if (nestedSegment.SegmentFinished is false)
				{
					nestedSegment.End(parentEnd);
				}
			}

			SegmentFinished = true;
		}

		/// <summary>
		/// Adds and starts nested segment.
		/// </summary>
		/// <param name="segmentName"></param>
		/// <returns></returns>
		public DurationSegmentBuilder StartNewNestedSegment(string segmentName, DateTimeOffset start)
		{

			var nestedSegment = new DurationSegmentBuilder(segmentName, start);
			nestedSegmentBuilders.Add(nestedSegment);
			return nestedSegment;
		}

		/// <summary>
		/// Adds and starts nested segment.
		/// </summary>
		/// <param name="segmentName"></param>
		/// <returns></returns>
		public DurationSegmentBuilder StartNewNestedSegment(string segmentName)
		{
			return StartNewNestedSegment(segmentName, DateTimeOffset.UtcNow);
		}

		public void Dispose()
		{
			End();
		}
	}
}