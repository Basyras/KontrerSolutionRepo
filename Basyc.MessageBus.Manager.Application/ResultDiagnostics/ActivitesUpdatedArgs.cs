using Basyc.Diagnostics.Shared.Logging;
using System.Diagnostics;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public record class ActivitesUpdatedArgs(ActivityEntry[] NewActivities);
}
