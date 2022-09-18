﻿using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.FluentApi
{
	public class FluentSetupNoReturnStage : BuilderStageBase
	{
		private readonly InProgressMessageRegistration inProgressMessage;
		private readonly InProgressDomainRegistration inProgressDomain;

		public FluentSetupNoReturnStage(IServiceCollection services, InProgressMessageRegistration inProgressMessage, InProgressDomainRegistration inProgressDomain) : base(services)
		{
			this.inProgressMessage = inProgressMessage;
			this.inProgressDomain = inProgressDomain;
		}

		public FluentSetupDomainPostStage HandeledBy(Action<RequestResult> handler)
		{
			inProgressMessage.RequestHandler = handler;
			return new FluentSetupDomainPostStage(services, inProgressDomain);
		}

		public FluentSetupDomainPostStage HandeledBy(Action<Request> handler)
		{
			Action<RequestResult> handlerWrapper = (requestResult) =>
			{
				requestResult.Start();
				handler.Invoke(requestResult.Request);
				requestResult.Complete();
			};
			inProgressMessage.RequestHandler = handlerWrapper;
			return new FluentSetupDomainPostStage(services, inProgressDomain);
		}
	}
}