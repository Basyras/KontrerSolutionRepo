using Basyc.DependencyInjection;
using Basyc.MessageBus.Manager.Infrastructure.MessageRegistration.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basyc.MessageBus.Manager.Infrastructure.Building.Interface
{
	public class SetupHasResponseStage : BuilderStageBase
	{
		private readonly InterfaceRegistration registration;

		public SetupHasResponseStage(IServiceCollection services, InterfaceRegistration registration) : base(services)
		{
			this.registration = registration;
		}

		public void NoResponse()
		{
			registration.HasResponse = false;
		}

		public SetupResponseStage HasResponse(Type responseType)
		{
			registration.HasResponse = true;
			registration.ResponseType = responseType;
			return new(services, registration);
		}

		public SetupResponseStage HasResponse<TResponse>()
		{
			return HasResponse(typeof(TResponse));
		}
	}
}
