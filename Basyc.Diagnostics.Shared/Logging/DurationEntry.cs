namespace Basyc.Diagnostics.Shared.Logging
{
	public record struct DurationEntry(string durationName, DateTimeOffset start, DateTimeOffset end);
}
