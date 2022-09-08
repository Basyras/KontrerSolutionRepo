using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration
{
	public class InProgressDomainRegistration
	{
		public string? DomainName { get; set; }
		public List<InProgressMessageRegistration> InProgressMessages { get; } = new List<InProgressMessageRegistration>();

	}
}
