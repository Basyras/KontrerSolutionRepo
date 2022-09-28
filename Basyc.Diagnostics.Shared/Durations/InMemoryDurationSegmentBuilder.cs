namespace Basyc.Diagnostics.Shared.Durations
{
	public class InMemoryDurationSegmentBuilder : DurationSegmentBuilderBase, IDisposable, IDurationSegmentBuilder
	{
		private readonly List<InMemoryDurationSegmentBuilder> nestedSegmentBuilders = new List<InMemoryDurationSegmentBuilder>();
		private readonly Func<InMemoryDurationSegmentBuilder>? parentSegmentGetter;
		public InMemoryDurationSegmentBuilder(string name) : base(name)
		{
			Name = name;
		}

		public InMemoryDurationSegmentBuilder(string name, Func<InMemoryDurationSegmentBuilder> parentSegmentGetter)
			: this(name)
		{
			this.parentSegmentGetter = parentSegmentGetter;
			HasParent = true;
		}

		public InMemoryDurationSegmentBuilder(string name, DateTimeOffset segmentStart, InMemoryDurationSegmentBuilder parentSegment)
			: this(name, () => parentSegment)
		{
			StartTime = segmentStart;
			HasStarted = true;
		}

		/// <summary>
		/// Ensures that <see cref="End"/> was called and produce <see cref="BuildedDurationSegment"/> with processed timestamps
		/// </summary>
		public BuildedDurationSegment Build()
		{
			var wasStarted = EnsureStarted(out var starTtime);

			if (HasEnded is false)
			{
				End();
			}

			return Build(EndTime);
		}

		/// <summary>
		/// Ensures that <see cref="End"/> was called and produce <see cref="BuildedDurationSegment"/> with processed timestamps
		/// </summary>
		public BuildedDurationSegment Build(DateTimeOffset finalEndTime)
		{
			if (EnsureStarted(finalEndTime) is false)
			{
				if (finalEndTime < StartTime)
				{
					throw new ArgumentException("Final end time can't be sooner than actual startTime time");
				}
			}

			if (HasEnded is false)
			{
				End(finalEndTime);
			}
			else
			{
				if (finalEndTime < EndTime)
				{
					throw new ArgumentException("Final end time can't be sooner than actual end time");
				}
			}

			return BuildNestedSegments();
		}

		private BuildedDurationSegment BuildNestedSegments()
		{
			BuildedDurationSegment[] nestedSegments = new BuildedDurationSegment[nestedSegmentBuilders.Count];
			for (int nestedSegmentIndex = 0; nestedSegmentIndex < nestedSegmentBuilders.Count; nestedSegmentIndex++)
			{
				InMemoryDurationSegmentBuilder? nestedSegmentBuilder = nestedSegmentBuilders[nestedSegmentIndex];
				var nestedSegment = nestedSegmentBuilder.Build(EndTime);
				nestedSegments[nestedSegmentIndex] = nestedSegment;
			}
			return new BuildedDurationSegment(Name, StartTime, EndTime, EndTime - StartTime, nestedSegments);
		}
		public override void End(DateTimeOffset finalEndTime)
		{
			if (HasEnded)
			{
				throw new InvalidOperationException($"{nameof(End)} was called twice");
			}

			EndTime = finalEndTime;

			foreach (var nestedSegment in nestedSegmentBuilders)
			{
				if (nestedSegment.HasEnded is false)
				{
					nestedSegment.End(finalEndTime);
				}
			}

			HasEnded = true;
		}

		public override IDurationSegmentBuilder EndAndStartNewFollowingSegment(string segmentName)
		{
			var endTime = DateTimeOffset.UtcNow;
			End(endTime);
			if (HasParent is false)
			{
				throw new InvalidOperationException("Cannot create following segment because this segment deos not have a parent");
			}
			return parentSegmentGetter!.Invoke().StartNewNestedSegment(segmentName, endTime);
		}

		public override IDurationSegmentBuilder StartNewNestedSegment(string segmentName, DateTimeOffset start)
		{
			EnsureStarted(start);
			var nestedSegment = new InMemoryDurationSegmentBuilder(segmentName, start, this);
			nestedSegmentBuilders.Add(nestedSegment);
			return nestedSegment;
		}

		public override IDurationSegmentBuilder StartNewNestedSegment(string segmentName)
		{
			var wasStarted = EnsureStarted(out var rootStartTime);
			return StartNewNestedSegment(segmentName, wasStarted ? DateTimeOffset.UtcNow : rootStartTime);
		}

	}
}