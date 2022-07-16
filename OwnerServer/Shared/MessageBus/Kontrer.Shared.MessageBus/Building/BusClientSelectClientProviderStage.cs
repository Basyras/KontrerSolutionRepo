using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Client.Building
{
	public class BusClientSelectClientProviderStage : BuilderStageBase
	{
		public BusClientSelectClientProviderStage(IServiceCollection services, MessageTypeOptions messageType) : base(services)
		{
			MessageType = messageType;
		}

		public MessageTypeOptions MessageType { get; }
	}
}
