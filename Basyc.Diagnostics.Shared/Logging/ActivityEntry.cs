using Basyc.Diagnostics.Shared.Durations;
using System.Diagnostics;

namespace Basyc.Diagnostics.Shared.Logging
{
	public record struct ActivityEntry(ServiceIdentity Service, string TraceId, string Name, DateTimeOffset StartTime, DateTimeOffset EndTime, ActivityStatusCode Status);
}
