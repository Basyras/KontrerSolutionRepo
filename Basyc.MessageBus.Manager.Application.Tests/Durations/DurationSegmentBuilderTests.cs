using Basyc.MessageBus.Manager.Application.Durations;

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
			segmentBuilder.SegmentFinished.Should().BeTrue();
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
			allBuilders.All(x => x.SegmentFinished).Should().BeTrue();
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
	}
}
