using System.Text;

namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Shared.Helpers
{
	public static class ColorHelper
	{
		public static string GetColorFromText(string textInput, double saturation, double saturationRandomness = 0, double opacity = 1)
		{

			if (saturation < 0 || saturation > 1)
				throw new ArgumentException("bad value", nameof(saturation));

			if (opacity < 0 || opacity > 1)
				throw new ArgumentException("bad value", nameof(opacity));

			if (saturationRandomness < 0 || saturationRandomness > 1)
				throw new ArgumentException("bad value", nameof(saturationRandomness));

			int saturationRandomness255 = (int)Math.Round(255 * saturationRandomness);
			int saturation255 = (int)Math.Round(255 * saturation);



			int seed = textInput.Select(x => (int)x).Sum();
			var random = new Random(seed);

			var remainingColours = new List<int>(3) { 0, 1, 2 };
			int[] colours = new int[3];
			int firstIndex = random.Next(0, 2);
			int randomSaturationToApply = random.Next(0, saturationRandomness255);
			colours[remainingColours[firstIndex]] = 255 - randomSaturationToApply;
			remainingColours.RemoveAt(firstIndex);

			int secondIndex = remainingColours[random.Next(0, 1)];
			randomSaturationToApply = random.Next(0, saturationRandomness255);
			colours[remainingColours[secondIndex]] = saturation255 - randomSaturationToApply;
			remainingColours.RemoveAt(secondIndex);

			int flexibleSaturation = random.Next(saturation255, 255);
			randomSaturationToApply = random.Next(0, saturationRandomness255);
			colours[remainingColours[0]] = flexibleSaturation - randomSaturationToApply;

			StringBuilder stringBuilder = new StringBuilder(6);
			stringBuilder.Append('#');
			stringBuilder.Append(colours[0].ToString("X2"));
			stringBuilder.Append(colours[1].ToString("X2"));
			stringBuilder.Append(colours[2].ToString("X2"));
			stringBuilder.Append(opacity.ToString("X2"));
			string finalColor = stringBuilder.ToString();
			return finalColor;
		}
	}
}
