using Basyc.MessageBus.Manager.Application.Initialization;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Application
{
	public interface IBusManagerApplication
	{
		IReadOnlyList<DomainInfo> DomainInfos { get; }
		bool Loaded { get; }
		void Load();
	}
}