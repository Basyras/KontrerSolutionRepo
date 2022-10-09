namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Components.DurationMap
{
	public static class DurationViewHelper
	{
		public static double GetDurationAsRem(TimeSpan duration, double scale)
		{
			var minHeight = Math.Round(duration.TotalMilliseconds / scale);
			if (minHeight < 5)
			{
				minHeight = 5;
			}
			if (minHeight > 100)
			{
				minHeight = 100;
			}
			return minHeight;
		}

		public static double GetDifferenceAsRem(TimeSpan duration, double scale)
		{
			var minHeight = Math.Round(duration.TotalMilliseconds / scale);
			return minHeight;
		}

		public static double GetDifferenceAsRem(DateTimeOffset firstEndtime, DateTimeOffset secondStartTime, double scale)
		{
			return GetDifferenceAsRem(secondStartTime - firstEndtime, scale);
		}

	}
}
