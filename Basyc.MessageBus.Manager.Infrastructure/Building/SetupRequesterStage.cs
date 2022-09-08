using Basyc.DependencyInjection;
using Basyc.MessageBus.Manager.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager.Infrastructure.Building
{
	public class SetupRequesterStage : BuilderStageBase
	{
		public SetupRequesterStage(IServiceCollection services) : base(services)
		{
		}

		public SetupTypeFormattingStage SelectRequester<TRequester>() where TRequester : class, IRequester
		{
			services.AddSingleton<IRequester, TRequester>();
			return new SetupTypeFormattingStage(services);
		}

		public SetupTypeFormattingStage SelectTypedRequester()
		{
			services.AddSingleton<IRequester, BasycTypedMessageBusRequester>();
			return new SetupTypeFormattingStage(services);
		}

	}
}
