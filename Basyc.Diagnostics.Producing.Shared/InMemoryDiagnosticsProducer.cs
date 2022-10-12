﻿using Basyc.Diagnostics.Shared.Logging;

namespace Basyc.Diagnostics.Producing.Shared
{
	public class InMemoryDiagnosticsProducer : IDiagnosticsProducer
	{
		public InMemoryDiagnosticsProducer()
		{

		}

		public Task ProduceLog(LogEntry logEntry)
		{
			LogProduced?.Invoke(this, logEntry);
			return Task.CompletedTask;
		}


		public Task StartActivity(ActivityStart activityStart)
		{
			StartProduced?.Invoke(this, activityStart);
			return Task.CompletedTask;

		}

		public Task EndActivity(ActivityEnd activityEnd)
		{
			EndProduced?.Invoke(this, activityEnd);
			return Task.CompletedTask;
		}

		public Task<bool> StartAsync()
		{
			return Task.FromResult(true);

		}

		public event EventHandler<LogEntry>? LogProduced;
		public event EventHandler<ActivityStart>? StartProduced;
		public event EventHandler<ActivityEnd>? EndProduced;
	}
}
