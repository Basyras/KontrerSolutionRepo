using Basyc.DependencyInjection;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Basyc.MessageBus.Manager.Infrastructure.Basyc;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager.Infrastructure.Building
{
	public class SetupDiagnosticsStage : BuilderStageBase
	{
		public SetupDiagnosticsStage(IServiceCollection services) : base(services)
		{
		}

		public SetupRequesterStage NoDiagnostics()
		{
			return new SetupRequesterStage(services);
		}

		public SetupRequesterStage UseBasycDiagnosticsReceivers()
		{
			services.AddSingleton<ILogSource, BasycReceiversLogSource>();
			return new SetupRequesterStage(services);
		}
	}
}
