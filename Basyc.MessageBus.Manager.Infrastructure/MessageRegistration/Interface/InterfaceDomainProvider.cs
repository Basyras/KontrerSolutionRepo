using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Application.Requesting;
using Basyc.MessageBus.Manager.Infrastructure.Basyc.Basyc.MessageBus;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Infrastructure.MessageRegistration.Interface
{
	public class InterfaceDomainProvider : IDomainInfoProvider
	{
		private readonly IOptions<InterfaceDomainProviderOptions> options;
		private readonly IRequesterSelector requesterSelector;
		private readonly IRequestInfoTypeStorage requestInfoTypeStorage;

		public InterfaceDomainProvider(IOptions<InterfaceDomainProviderOptions> options, IRequesterSelector requesterSelector, IRequestInfoTypeStorage requestInfoTypeStorage)
		{
			this.options = options;
			this.requesterSelector = requesterSelector;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
		}

		public List<DomainInfo> GenerateDomainInfos()
		{
			//var domains = new List<DomainInfo>();
			var domains = new Dictionary<string, List<RequestInfo>>();

			foreach (var registration in options.Value.InterfaceRegistrations)
			{
				domains.TryAdd(registration.DomainName, new List<RequestInfo>());
				var infos = domains[registration.DomainName];
				foreach (var assembly in registration.AssembliesToScan)
				{
					foreach (var type in assembly.GetTypes())
					{
						var implementsInterface = type.GetInterface(registration.MessageInterfaceType.Name) is not null;
						if (implementsInterface is false)
							continue;

						List<ParameterInfo> paramInfos = TypedProviderHelper.HarvestParameterInfos(type, x => x.Name);
						string messageDisplayName = registration.DisplayNameFormatter.Invoke(type);
						RequestInfo requestInfo = registration.HasResponse
							? new RequestInfo(registration.RequestType, paramInfos, registration.ResponseType, messageDisplayName, registration.ResponseDisplayName)
							: new RequestInfo(registration.RequestType, paramInfos, messageDisplayName);
						requesterSelector.AssignRequester(requestInfo, BasycTypedMessageBusRequester.BasycTypedMessageBusRequesterUniqueName);
						requestInfoTypeStorage.AddRequest(requestInfo, type);
						infos.Add(requestInfo);
					}
				}

			}

			var domainInfos = domains.Select(x => new DomainInfo(x.Key, x.Value)).ToList();
			return domainInfos;
		}
	}
}
