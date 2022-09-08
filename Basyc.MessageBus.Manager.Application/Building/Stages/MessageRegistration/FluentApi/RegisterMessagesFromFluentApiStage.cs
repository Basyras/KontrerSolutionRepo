using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.FluentApi
{
	public class RegisterMessagesFromFluentApiStage : BuilderStageBase
	{
		public RegisterMessagesFromFluentApiStage(IServiceCollection services) : base(services)
		{
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
