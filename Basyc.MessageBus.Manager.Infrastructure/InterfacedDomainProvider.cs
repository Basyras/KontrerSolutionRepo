using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.Shared.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Infrastructure
{
	public class InterfacedDomainProvider : IDomainInfoProvider
	{
		private readonly ITypedDomainNameFormatter domainNameFormatter;
		private readonly ITypedRequestNameFormatter requestNameFormatter;
		private readonly ITypedParameterNameFormatter parameterNameFormatter;
		private readonly ITypedResponseNameFormatter responseNameFormatter;
		private readonly IRequestInfoTypeStorage requestInfoTypeStorage;
		private readonly IOptions<InterfacedDomainProviderOptions> options;
		private readonly IRequesterSelector requesterSelector;

		public InterfacedDomainProvider(
			ITypedDomainNameFormatter domainNameFormatter,
			ITypedRequestNameFormatter requestNameFormatter,
			ITypedParameterNameFormatter parameterNameFormatter,
			ITypedResponseNameFormatter responseNameFormatter,
			IRequestInfoTypeStorage requestInfoTypeStorage,
			IOptions<InterfacedDomainProviderOptions> options,
			IRequesterSelector requesterSelector
			)
		{
			this.parameterNameFormatter = parameterNameFormatter;
			this.responseNameFormatter = responseNameFormatter;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
			this.domainNameFormatter = domainNameFormatter;
			this.requestNameFormatter = requestNameFormatter;
			this.options = options;
			this.requesterSelector = requesterSelector;
		}

		public List<DomainInfo> GenerateDomainInfos()
		{
			var domains = new List<DomainInfo>();

			foreach (var assemblyWithMessages in options.Value.AssembliesToScan)
			{
				var requestInfos = new List<RequestInfo>();
				foreach (var type in assemblyWithMessages.GetTypes())
				{
					if (TryParseRequestType(type, out RequestType requestType, out bool hasResponse, out Type responseType))
					{
						List<Application.Initialization.ParameterInfo> paramInfos = TypedProviderHelper.HarvestParameterInfos(type, parameterNameFormatter);

						RequestInfo requestInfo = hasResponse
							? new RequestInfo(requestType, paramInfos, responseType, requestNameFormatter.GetFormattedName(type), responseNameFormatter.GetFormattedName(responseType))
							: new RequestInfo(requestType, paramInfos, requestNameFormatter.GetFormattedName(type));
						requestInfos.Add(requestInfo);

						requesterSelector.AssignRequester(requestInfo, BasycTypedMessageBusRequester.BasycTypedMessageBusRequesterUniqueName);
						requestInfoTypeStorage.AddRequest(requestInfo, type);
					}
				}

				domains.Add(new DomainInfo(domainNameFormatter.GetFormattedName(assemblyWithMessages), requestInfos));
			}
			return domains;
		}

		/// <summary>
		/// False if type is not a request
		/// </summary>
		/// <param name="type"></param>
		/// <param name="requesType"></param>
		/// <param name="hasResponse"></param>
		/// <param name="responseType"></param>
		/// <returns></returns>
		private bool TryParseRequestType(Type type, out RequestType requesType, out bool hasResponse, out Type responseType)
		{
			responseType = null;
			hasResponse = false;
			requesType = default;

			if (type.IsClass is false)
				return false;
			if (type.IsAbstract is true)
				return false;

			if (options.Value.IQueryType is not null && type.GetInterface(options.Value.IQueryType.Name) is not null)
			{
				requesType = RequestType.Query;
				hasResponse = true;
				responseType = GenericsHelper.GetTypeArgumentsFromParent(type, options.Value.IQueryType)[0];
				return true;
			}

			if (options.Value.ICommandType is not null && type.GetInterface(options.Value.ICommandType.Name) is not null)
			{
				requesType = RequestType.Command;
				return true;
			}

			if (options.Value.ICommandWithResponseType is not null && type.GetInterface(options.Value.ICommandWithResponseType.Name) is not null)
			{
				requesType = RequestType.Command;
				hasResponse = true;
				responseType = GenericsHelper.GetTypeArgumentsFromParent(type, options.Value.ICommandWithResponseType)[0];
				return true;
			}

			if (options.Value.IMessageType is not null && type.GetInterface(options.Value.IMessageType.Name) is not null)
			{
				requesType = RequestType.Generic;
				return true;
			}

			if (options.Value.IMessageWithResponseType is not null && type.GetInterface(options.Value.IMessageWithResponseType.Name) is not null)
			{
				requesType = RequestType.Generic;
				hasResponse = true;
				responseType = GenericsHelper.GetTypeArgumentsFromParent(type, options.Value.IMessageWithResponseType)[0];
				return true;
			}

			return false;
		}
	}
}