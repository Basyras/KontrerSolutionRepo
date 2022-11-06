using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application
{
	public class FluentApiDomainInfoProviderOptions
	{
		public List<InProgressDomainRegistration> DomainRegistrations { get; } = new List<InProgressDomainRegistration>();
	}
}
