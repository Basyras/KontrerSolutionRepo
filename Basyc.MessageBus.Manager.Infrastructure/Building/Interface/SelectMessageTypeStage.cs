using Basyc.DependencyInjection;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure.MessageRegistration.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager.Infrastructure.Building.Interface
{
	public class SelectMessageTypeStage : BuilderStageBase
	{
		private readonly InterfaceRegistration interfaceRegistration;

		public SelectMessageTypeStage(IServiceCollection services, InterfaceRegistration interfaceRegistration) : base(services)
		{
			this.interfaceRegistration = interfaceRegistration;
		}

		public void AsEvents()
		{
			interfaceRegistration.RequestType = RequestType.Event;
			//return new SetupResponseStage(services, interfaceRegistration);
		}

		public SetupHasResponseStage AsRequests()
		{
			interfaceRegistration.RequestType = RequestType.Query;
			return new SetupHasResponseStage(services, interfaceRegistration);
		}

		public SetupHasResponseStage AsCommand()
		{
			interfaceRegistration.RequestType = RequestType.Command;
			return new SetupHasResponseStage(services, interfaceRegistration);
		}
	}
}
