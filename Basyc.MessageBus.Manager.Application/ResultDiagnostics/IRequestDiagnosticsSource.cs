using System;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface IRequestDiagnosticsSource
	{
		event EventHandler<LogsUpdatedArgs> LogsReceived;
		event EventHandler<ActivitesUpdatedArgs> ActivitiesReceived;
	}
}
