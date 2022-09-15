namespace Basyc.Diagnostics.Shared.Durations
{
	public class DurationSegmentBuilder : IDisposable
	{
		private readonly List<DurationSegmentBuilder> nestedSegmentBuilders = new List<DurationSegmentBuilder>();
		private readonly Func<DurationSegmentBuilder>? parentSegmentGetter;
		private readonly bool hasParent;

		public DateTimeOffset EndTime { get; private set; }
		public DateTimeOffset StartTime { get; init; }
		public bool SegmentEnded { get; private set; }
		public string Name { get; init; }


		public DurationSegmentBuilder(string name, DateTimeOffset segmentStart)
		{
			Name = name;
			StartTime = segmentStart;
		}

		public DurationSegmentBuilder(string name, DateTimeOffset segmentStart, DurationSegmentBuilder parentSegment)
			: this(name, segmentStart, () => parentSegment)
		{
		}

		public DurationSegmentBuilder(string name, DateTimeOffset segmentStart, Func<DurationSegmentBuilder> parentSegmentGetter)
			: this(name, segmentStart)
		{
			this.parentSegmentGetter = parentSegmentGetter;
			hasParent = true;
		}


		/// <summary>
		/// Ensures that <see cref="End"/> was called and produce <see cref="DurationSegment"/> with processed timestamps
		/// </summary>
		public DurationSegment Build()
		{
			if (SegmentEnded is false)
			{
				End();
			}

			return Build(EndTime);
		}

		/// <summary>
		/// Ensures that <see cref="End"/> was called and produce <see cref="DurationSegment"/> with processed timestamps
		/// </summary>
		public DurationSegment Build(DateTimeOffset finalEndTime)
		{
			if (SegmentEnded is false)
			{
				End(finalEndTime);
			}

			return BuildNestedSegments();
		}

		private DurationSegment BuildNestedSegments()
		{
			DurationSegment[] nestedSegments = new DurationSegment[nestedSegmentBuilders.Count];
			for (int nestedSegmentIndex = 0; nestedSegmentIndex < nestedSegmentBuilders.Count; nestedSegmentIndex++)
			{
				DurationSegmentBuilder? nestedSegmentBuilder = nestedSegmentBuilders[nestedSegmentIndex];
				var nestedSegment = nestedSegmentBuilder.Build(EndTime);
				nestedSegments[nestedSegmentIndex] = nestedSegment;
			}
			return new DurationSegment(Name, StartTime, EndTime, EndTime - StartTime, nestedSegments);
		}

		public DateTimeOffset End()
		{
			End(DateTimeOffset.UtcNow);
			return EndTime;
		}

		public void End(DateTimeOffset finalEndTime)
		{
			if (SegmentEnded)
			{
				throw new InvalidOperationException($"{nameof(End)} was called twice");
			}

			EndTime = finalEndTime;

			foreach (var nestedSegment in nestedSegmentBuilders)
			{
				if (nestedSegment.SegmentEnded is false)
				{
					nestedSegment.End(finalEndTime);
				}
			}

			SegmentEnded = true;
		}

		/// <summary>
		/// End current segment and craetes following segment (Not nested segment). 
		/// This ensures that there is not a gap between end and start of the new following segment
		/// Call only when segment has a parent!
		/// </summary>
		/// <param name="segmentName"></param>
		/// <returns></returns>
		public DurationSegmentBuilder EndAndStartNewFollowingSegment(string segmentName)
		{
			var endTime = DateTimeOffset.UtcNow;
			End(endTime);
			if (hasParent is false)
			{
				throw new InvalidOperationException("Cannot create following segment because this segment deos not have a parent");
			}
			return parentSegmentGetter!.Invoke().StartNewNestedSegment(segmentName, endTime);
		}

		public DurationSegmentBuilder StartNewNestedSegment(string segmentName, DateTimeOffset start)
		{
			var nestedSegment = new DurationSegmentBuilder(segmentName, start, this);
			nestedSegmentBuilders.Add(nestedSegment);
			return nestedSegment;
		}

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