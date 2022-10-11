namespace Basyc.Diagnostics.Shared.Logging
{
	public record struct DiagnosticChange(LogEntry[] Logs, ActivityStart[] ActivityStarts, ActivityEnd[] ActivityEnds);
}
