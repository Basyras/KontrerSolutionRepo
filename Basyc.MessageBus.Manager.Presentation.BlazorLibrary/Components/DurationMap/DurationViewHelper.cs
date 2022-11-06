using System.Globalization;

namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Components.DurationMap
{
	public static class DurationViewHelper
	{
		private readonly static NumberFormatInfo numberFormatter;
		static DurationViewHelper()
		{
			numberFormatter = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
			numberFormatter.NumberDecimalSeparator = ".";
		}
		/// <summary>
		/// Formats the number and append rem unit. Example output "15.8rem"
		/// </summary>
		public static string GetCssDurationValue(TimeSpan duration, double scale, out double lenghtNumber)
		{
			//var minHeight = (duration.TotalMilliseconds / 20) * scale;
			//remNumber = minHeight;
			//return $"{minHeight.ToString(numberFormatter)}rem";

			var displayLenght = Math.Round(duration.TotalMilliseconds) * scale;
			lenghtNumber = displayLenght;
			return $"{displayLenght.ToString(numberFormatter)}px";
		}

		/// <summary>
		/// Formats the number and append rem unit. Example output "15.8rem"
		/// </summary>
		public static string GetCssDurationValue(TimeSpan duration, double scale)
		{
			return GetCssDurationValue(duration, scale, out _);
		}

		/// <summary>
		/// Formats the number and append rem unit. Example output "15.8rem"
		/// </summary>
		public static string GetCssDurationValue(DateTimeOffset earlyTime, DateTimeOffset laterTime, double scale, out double remNumber)
		{
			return GetCssDurationValue(laterTime - earlyTime, scale, out remNumber);
		}

		/// <summary>
		/// Formats the number and append rem unit. Example output "15.8rem"
		/// </summary>
		public static string GetCssDurationValue(DateTimeOffset earlyTime, DateTimeOffset laterTime, double scale)
		{
			return GetCssDurationValue(earlyTime, laterTime, scale, out _);
		}

	}
}
