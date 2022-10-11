namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Components.DurationMap
{
	public static class DurationViewHelper
	{
		public static double GetDurationAsRem(TimeSpan duration, double scale)
		{
			var minHeight = duration.TotalMilliseconds / scale;
			return minHeight;
		}

		public static double GetDurationAsRem(DateTimeOffset earlyTime, DateTimeOffset laterTime, double scale)
		{
			return GetDurationAsRem(laterTime - earlyTime, scale);
		}

	}
}
