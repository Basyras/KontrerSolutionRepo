using Basyc.MessageBus.Manager.Application.Initialization;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application
{
	public class BusManagerApplication : IBusManagerApplication
	{
		private readonly IDomainInfoProvider messageDomainLoader;
		public IReadOnlyList<DomainInfo> DomainInfos { get; private set; }

		public bool Loaded { get; private set; }

		public BusManagerApplication(IDomainInfoProvider messageDomainLoader)
		{
			this.messageDomainLoader = messageDomainLoader;
		}

		public void Load()
		{
			DomainInfos = messageDomainLoader.GetDomainInfos();
			Loaded = true;
		}
	}
}