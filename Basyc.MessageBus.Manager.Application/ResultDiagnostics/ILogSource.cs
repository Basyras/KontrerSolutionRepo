using System;

namespace Basyc.MessageBus.Manager.Application.ResultDiagnostics
{
	public interface ILogSource
	{
		event EventHandler<LogsUpdatedArgs> LogsReceived;
	}
}
