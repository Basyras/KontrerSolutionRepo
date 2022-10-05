namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Components.DurationMap
{
	public static class DurationViewHelper
	{
		public static double GetDurationAsRem(TimeSpan duration, double scale)
		{
			var minHeight = Math.Round(duration.TotalMilliseconds / scale);
			if (minHeight < 1)
			{
				minHeight = 1;
			}
			if (minHeight > 25)
			{
				minHeight = 25;
			}
			return minHeight;
		}

	}
}
