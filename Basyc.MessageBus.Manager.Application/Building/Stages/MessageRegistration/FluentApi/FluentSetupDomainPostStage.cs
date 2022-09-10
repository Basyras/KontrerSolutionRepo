﻿using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.FluentApi
{
	public class FluentSetupDomainPostStage : BuilderStageBase
	{
		private readonly InProgressDomainRegistration inProgressDomain;

		public FluentSetupDomainPostStage(IServiceCollection services, InProgressDomainRegistration inProgressDomain) : base(services)
		{
			this.inProgressDomain = inProgressDomain;
		}

		public FluentSetupMessageStage AddMessage(string messageDisplayName, RequestType messageType = RequestType.Generic)
		{
			return new FluentSetupDomainStage(services, inProgressDomain).AddMessage(messageDisplayName, messageType);
		}

		public FluentSetupDomainStage AddDomain(string domainName)
		{
			return new RegisterMessagesFromFluentApiStage(services).AddDomain(domainName);
		}
	}
}
