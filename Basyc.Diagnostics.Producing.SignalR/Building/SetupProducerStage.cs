using Basyc.DependencyInjection;
using Basyc.Diagnostics.Receiving.SignalR;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public class SetupProducerStage : BuilderStageBase
	{
		public SetupProducerStage(IServiceCollection services) : base(services)
		{
		}

		public void UseOptions(Action<SignalRLogReceiverOptions> optionSetup)
		{
			services.Configure<SignalRLogReceiverOptions>(optionSetup);
		}

		public void UseConfiguration(IConfiguration configuration)
		{
			var sec = configuration.GetSection(nameof(SignalRLogReceiverOptions));
			services.Configure<SignalRLogReceiverOptions>(sec, o =>
			{
				o.ErrorOnUnknownConfiguration = true;
			});
		}

	}
}