using Basyc.MessageBus.Manager.Application.Initialization;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application
{
	public class BusManagerApplication : IBusManagerApplication
	{
		private readonly IDomainInfoProvider[] messageDomainLoaders;
		public IReadOnlyList<DomainInfo>? DomainInfos { get; private set; }

		public bool Loaded { get; private set; }

		public BusManagerApplication(IEnumerable<IDomainInfoProvider> messageDomainLoaders)
		{
			this.messageDomainLoaders = messageDomainLoaders.ToArray();
		}

		public void Load()
		{
			DomainInfos = messageDomainLoaders.SelectMany(x => x.GenerateDomainInfos()).ToList();
			Loaded = true;
		}
	}
}