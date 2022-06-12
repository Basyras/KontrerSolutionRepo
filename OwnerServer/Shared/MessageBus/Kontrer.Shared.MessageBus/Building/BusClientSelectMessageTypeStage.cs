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
			services.AddSingleton<ITypedMessageBusClient, TypedFromSimpleMessageBusClient>();
			return new BusClientSetupTypedHandlersStage(services);
		}

		public BusClientSetupTypedHandlersStage WithSimpleMessages()
		{
			return new BusClientSetupTypedHandlersStage(services);
		}

		public BusClientSetupTypedHandlersStage WithByteMessages()
		{
			services.AddSingleton<IByteMessageBusClient, ByteFromSimpleMessageBusClient>();
			return new BusClientSetupTypedHandlersStage(services);
		}
	}
}