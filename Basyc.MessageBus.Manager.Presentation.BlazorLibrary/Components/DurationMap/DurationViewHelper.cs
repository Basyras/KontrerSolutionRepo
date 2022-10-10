namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Components.DurationMap
{
	public static class DurationViewHelper
	{
		public static double GetDurationAsRem(TimeSpan duration, double scale)
		{
			var minHeight = Math.Round(duration.TotalMilliseconds / scale);
			//if (minHeight < 5)
			//{
			//	minHeight = 5;
			//}
			//if (minHeight > 100)
			//{
			//	minHeight = 100;
			//}
			return minHeight;
		}

		public static double GetDurationAsRem(DateTimeOffset earlyTime, DateTimeOffset laterTime, double scale)
		{
			return GetDurationAsRem(laterTime - earlyTime, scale);
		}

	}
}
