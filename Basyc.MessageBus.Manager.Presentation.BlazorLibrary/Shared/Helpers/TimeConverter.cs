namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Shared.Helpers
{
	public static class TimeConverter
	{
		public static string DurationToText(TimeSpan duration)
		{
			return $"{Math.Ceiling(duration.TotalMilliseconds)} ms";
		}

		public static string TimeToText(DateTime dateTime)
		{
			return dateTime.ToString("hh:mm:ss:ffff");
		}

		public static string TimeToText(DateTimeOffset dateTimeOffset)
		{
			return dateTimeOffset.LocalDateTime.ToString("hh:mm:ss:ffff");
		}
	}
}
