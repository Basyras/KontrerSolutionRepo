using Basyc.MessageBus.Manager.Application.Initialization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Basyc.MessageBus.Manager.Application.Requesting
{
	public class RequesterSelector : IRequesterSelector
	{
		private readonly Dictionary<string, IRequester> requesterToChoose;
		private readonly Dictionary<RequestInfo, string> infoToRequesterNameMap;
		private readonly IOptions<RequesterSelectorOptions> options;

		public RequesterSelector(IEnumerable<IRequester> requesters, IOptions<RequesterSelectorOptions> options)
		{
			requesterToChoose = requesters.ToDictionary(x => x.UniqueName, x => x);
			this.options = options;
			infoToRequesterNameMap = options.Value.ResolveRequesterMap();
		}

		public IRequester PickRequester(RequestInfo requestInfo)
		{
			var requesterName = infoToRequesterNameMap[requestInfo];
			return requesterToChoose[requesterName];
		}

		public void AssignRequester(RequestInfo requestInfo, string requesterUniqueName)
		{
			infoToRequesterNameMap.Add(requestInfo, requesterUniqueName);
		}
	}
}
