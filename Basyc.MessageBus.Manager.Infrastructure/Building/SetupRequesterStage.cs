using Basyc.DependencyInjection;
using Basyc.MessageBus.Manager.Application.Requesting;
using Basyc.MessageBus.Manager.Infrastructure.Basyc.Basyc.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager.Infrastructure.Building
{
	public class SetupRequesterStage : BuilderStageBase
	{
		public SetupRequesterStage(IServiceCollection services) : base(services)
		{
		}

		public SetupTypeFormattingStage UseBasycTypedMessageBusRequester()
		{
			services.AddSingleton<IRequester, BasycTypedMessageBusRequester>();
			return new SetupTypeFormattingStage(services);
		}

	}
}
