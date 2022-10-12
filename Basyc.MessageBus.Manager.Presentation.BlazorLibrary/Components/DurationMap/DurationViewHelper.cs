using System.Globalization;

namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Components.DurationMap
{
	public static class DurationViewHelper
	{
		/// <summary>
		/// Formats the number and append rem unit. Example output "15.8rem"
		/// </summary>
		public static string GetDurationAsRem(TimeSpan duration, double scale)
		{
			NumberFormatInfo numberFormatter = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
			numberFormatter.NumberDecimalSeparator = ".";
			var minHeight = (duration.TotalMilliseconds / 20) * scale;
			return $"{minHeight.ToString(numberFormatter)}rem";
		}

		/// <summary>
		/// Formats the number and append rem unit. Example output "15.8rem"
		/// </summary>
		public static string GetDurationAsRem(DateTimeOffset earlyTime, DateTimeOffset laterTime, double scale)
		{
			return GetDurationAsRem(laterTime - earlyTime, scale);
		}

	}
}
