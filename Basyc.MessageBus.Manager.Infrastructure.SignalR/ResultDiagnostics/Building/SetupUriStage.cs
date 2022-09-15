using Basyc.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager.Infrastructure.SignalR.ResultDiagnostics.Building
{
	public class SetupUriStage : BuilderStageBase
	{
		public SetupUriStage(IServiceCollection services) : base(services)
		{
		}

		public SetupUriStage UseConfiguration(IConfiguration configuration)
		{
			var sec = configuration.GetSection(nameof(SignalRLogSourceOptions));
			services.Configure<SignalRLogSourceOptions>(sec, o =>
			{
				o.ErrorOnUnknownConfiguration = true;
			});
			return this;
		}

		public void UseDefaults()
		{
			services.Configure<SignalRLogSourceOptions>(options =>
			{
				options.SignalRServerUri = "https://localhost:5182/logSource";
			});
		}

	}
}
