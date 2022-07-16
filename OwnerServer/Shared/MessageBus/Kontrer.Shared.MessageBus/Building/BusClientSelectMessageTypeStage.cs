using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Client.Building
{
	public class BusClientSelectMessageTypeStage : BuilderStageBase
	{

		public BusClientSelectMessageTypeStage(IServiceCollection services) : base(services)
		{

		}

		public BusClientSetupTypedHandlersStage WithTypedMessages()
		{
			return new BusClientSetupTypedHandlersStage(services, MessageTypeOptions.Typed);
		}

		public BusClientSetupTypedHandlersStage WithObjectMessages()
		{
			return new BusClientSetupTypedHandlersStage(services, MessageTypeOptions.Object);
		}

		public BusClientSetupTypedHandlersStage WithByteMessages()
		{
			return new BusClientSetupTypedHandlersStage(services, MessageTypeOptions.Byte);
		}
	}
}