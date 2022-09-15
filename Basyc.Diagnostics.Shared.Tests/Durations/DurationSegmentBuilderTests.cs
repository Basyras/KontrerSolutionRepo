using Basyc.Diagnostics.Shared.Durations;

namespace Basyc.MessageBus.Manager.Application.Tests.Durations
{
	public class DurationSegmentBuilderTests
	{
		[Fact]
		public void When_BuildingEmpty_Should_BeEmpty()
		{
			const string segmentName = "segmentName";
			var segmentBuilder = new DurationSegmentBuilder(segmentName, DateTimeOffset.UtcNow);
			segmentBuilder.End();
			segmentBuilder.SegmentEnded.Should().BeTrue();
			var segment = segmentBuilder.Build();

			segment.NestedSegments.Length.Should().Be(0);
			segment.StartTime.Should().NotBe(default(DateTimeOffset));
			segment.EndTime.Should().NotBe(default(DateTimeOffset));
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(1, 0)]
		[InlineData(1, 1)]
		[InlineData(3, 0)]
		[InlineData(3, 3)]
		[InlineData(2, 4)]
		[InlineData(4, 2)]
		public void When_Building_Should_FinishEvenNested(uint nestedSegmentsNumber, uint levelOfNesting)
		{
			DurationSegmentBuilder? rootSegmentBuilder = CreateNesting(nestedSegmentsNumber, levelOfNesting, out var allBuilders);
			var rootSegment = rootSegmentBuilder.Build();
			allBuilders.All(x => x.SegmentEnded).Should().BeTrue();
		}

		private DurationSegmentBuilder CreateNesting(uint nestedSegmentsNumber, uint levelOfNestingInNestedSegments, out List<DurationSegmentBuilder> allBuilders)
		{
			allBuilders = new List<DurationSegmentBuilder>();
			const string segmentName = "segmentName";
			var rootSegmentBuilder = new DurationSegmentBuilder("root", DateTimeOffset.UtcNow);
			allBuilders.Add(rootSegmentBuilder);

			for (int rootNestedSegmentIndex = 0; rootNestedSegmentIndex < nestedSegmentsNumber; rootNestedSegmentIndex++)
			{
				var nestedSegment = rootSegmentBuilder.StartNewNestedSegment(segmentName);
				allBuilders.Add(nestedSegment);

				for (int levelOfNestingIndex = 0; levelOfNestingIndex < levelOfNestingInNestedSegments; levelOfNestingIndex++)
				{
					nestedSegment = nestedSegment.StartNewNestedSegment(segmentName);
					allBuilders.Add(nestedSegment);
				}
			}
			uint levelOfNesteding = levelOfNestingInNestedSegments + 1;
			((uint)allBuilders.Count).Should().Be(1 + nestedSegmentsNumber * levelOfNesteding);

			return rootSegmentBuilder;
		}

		[Fact]
		public void EndAndStart_Should_HaveSameEndAndStarTimes()
		{
			var rootSegmentBuilder = new DurationSegmentBuilder("root", DateTimeOffset.UtcNow);
			var nestedSegmentBuilder1 = rootSegmentBuilder.StartNewNestedSegment("nested1");
			var nestedSegmentBuilder2 = nestedSegmentBuilder1.EndAndStartNewFollowingSegment("nested2");

			nestedSegmentBuilder1.SegmentEnded.Should().BeTrue();
			nestedSegmentBuilder2.SegmentEnded.Should().BeFalse();

			nestedSegmentBuilder1.EndTime.Should().Be(nestedSegmentBuilder2.StartTime);
			nestedSegmentBuilder2.EndTime.Should().Be(default(DateTimeOffset));

			var rootSegment = rootSegmentBuilder.Build();
			rootSegment.NestedSegments.Length.Should().Be(2);
			rootSegment.NestedSegments.All(x => x.NestedSegments.Length == 0).Should().BeTrue();
		}

		[Theory]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(2)]
		[InlineData(4)]
		public void StartNested_Should_AddNested(int nestedSegmentsNumber)
		{
			var rootSegmentBuilder = new DurationSegmentBuilder("root", DateTimeOffset.UtcNow);
			for (int segmentIndex = 0; segmentIndex < nestedSegmentsNumber; segmentIndex++)
			{
				rootSegmentBuilder.StartNewNestedSegment("nestedSegment");
			}

			var rootSegment = rootSegmentBuilder.Build();
			rootSegment.NestedSegments.Length.Should().Be(nestedSegmentsNumber);
		}


		[Theory]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(2)]
		[InlineData(4)]
		public void EndAndStart_Should_AddNestedToTheParent(int nestedSegmentsNumber)
		{
			var rootSegmentBuilder = new DurationSegmentBuilder("root", DateTimeOffset.UtcNow);
			var nestedSegment = rootSegmentBuilder.StartNewNestedSegment("nestedFollowing0");
			for (int segmentIndex = 1; segmentIndex < nestedSegmentsNumber; segmentIndex++)
			{
				var previousSegmentBuilder = nestedSegment;
				nestedSegment = nestedSegment.EndAndStartNewFollowingSegment($"nestedFollowing{segmentIndex}");
				previousSegmentBuilder.SegmentEnded.Should().BeTrue();
				nestedSegment.SegmentEnded.Should().BeFalse();
			}

			var rootSegment = rootSegmentBuilder.Build();
			rootSegment.NestedSegments.Length.Should().Be(nestedSegmentsNumber);
		}

		[Fact]
		public void When_DoesNotHaveParent_Should_ThrowWhenEndingAndStartingNew()
		{
			var rootSegmentBuilder = new DurationSegmentBuilder("root", DateTimeOffset.UtcNow);
			Assert.Throws<InvalidOperationException>(() => rootSegmentBuilder.EndAndStartNewFollowingSegment("followingSegment"));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(2)]
		[InlineData(4)]
		public void StartNested_When_NotEndingPrevious_Should_BeInParrarel(int segmentsInParrarel)
		{
			var rootSegmentBuilder = new DurationSegmentBuilder("root", DateTimeOffset.UtcNow);
			var nestedSegmentBuilder = rootSegmentBuilder.StartNewNestedSegment("nestedSegment");

			for (int segmentIndex = 0; segmentIndex < segmentsInParrarel - 1; segmentIndex++)
			{
				var previous = nestedSegmentBuilder;
				nestedSegmentBuilder = rootSegmentBuilder.StartNewNestedSegment("nestedSegment");
				nestedSegmentBuilder.SegmentEnded.Should().BeFalse();
				previous.SegmentEnded.Should().BeFalse();
				Thread.Sleep(150);
			}

			var rootSegment = rootSegmentBuilder.Build();
			rootSegment.NestedSegments.Length.Should().Be(segmentsInParrarel);
			rootSegment.NestedSegments.DistinctBy(x => x.StartTime).Count().Should().Be(rootSegment.NestedSegments.Length);
			rootSegment.NestedSegments.DistinctBy(x => x.EndTime).Count().Should().Be(1);
		}

	}
}
