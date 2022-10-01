using Basyc.Diagnostics.Shared.Logging;
using System.Diagnostics;

namespace Basyc.Diagnostics.Receiving.Abstractions
{
	public record ActivitiesReceivedArgs(ActivityEntry[] Activities);
}