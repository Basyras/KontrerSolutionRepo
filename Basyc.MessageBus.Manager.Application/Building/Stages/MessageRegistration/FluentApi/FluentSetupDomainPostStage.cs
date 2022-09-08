using Basyc.DependencyInjection;
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


		public FluentSetupMessageParemeterStage AddMessage(string messageDisplayName, RequestType messageType = RequestType.Generic)
		{
			var newMessage = new InProgressMessageRegistration();
			newMessage.MessagDisplayName = messageDisplayName;
			newMessage.MessageType = messageType;
			inProgressDomain.InProgressMessages.Add(newMessage);
			return new FluentSetupMessageParemeterStage(services, newMessage, inProgressDomain);
		}

		public FluentSetupDomainStage AddDomain(string domainName)
		{
			InProgressDomainRegistration newDomain = new InProgressDomainRegistration();
			newDomain.DomainName = domainName;
			services.Configure<FluentApiDomainInfoProviderOptions>(x => x.DomainRegistrations.Add(newDomain));
			return new FluentSetupDomainStage(services, newDomain);
		}
	}
}
