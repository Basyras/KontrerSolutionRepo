using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Infrastructure
{
    public class TypedDomainProvider : IDomainInfoProvider
    {
        private readonly IOptions<TypedDomainProviderOptions> options;
        private readonly ITypedDomainNameFormatter domainNameFormatter;
        private readonly ITypedRequestNameFormatter requestNameFormatter;
        private readonly ITypedParameterNameFormatter parameterNameFormatter;
        private readonly ITypedResponseNameFormatter responseNameFormatter;
        private readonly IRequestInfoTypeStorage requestInfoTypeStorage;

        public TypedDomainProvider(
            IOptions<TypedDomainProviderOptions> options,
            ITypedDomainNameFormatter domainNameFormatter,
            ITypedRequestNameFormatter requestNameFormatter,
            ITypedParameterNameFormatter parameterNameFormatter,
            ITypedResponseNameFormatter responseNameFormatter,
            IRequestInfoTypeStorage requestInfoTypeStorage)
        {
            this.options = options;
            this.domainNameFormatter = domainNameFormatter;
            this.requestNameFormatter = requestNameFormatter;
            this.parameterNameFormatter = parameterNameFormatter;
            this.responseNameFormatter = responseNameFormatter;
            this.requestInfoTypeStorage = requestInfoTypeStorage;
        }

        public List<DomainInfo> GetDomainInfos()
        {
            var domains = new List<DomainInfo>();
            foreach (var domainOption in options.Value.TypedDomainOptions)
            {
                var requestInfos = new List<RequestInfo>();
                foreach (var genericRequestType in domainOption.GenericRequestTypes)
                {
                    List<Application.Initialization.ParameterInfo> paramInfos = TypedProviderHelper.HarversParameterInfos(genericRequestType, parameterNameFormatter);
                    var requestInfo = new RequestInfo(RequestType.Command, paramInfos, requestNameFormatter.GetFormattedName(genericRequestType));
                    requestInfos.Add(requestInfo);
                    requestInfoTypeStorage.AddRequest(requestInfo, genericRequestType);
                }
                foreach (var typePair in domainOption.GenericRequestWithResponseTypes)
                {
                    List<Application.Initialization.ParameterInfo> paramInfos = TypedProviderHelper.HarversParameterInfos(typePair.RequestType, parameterNameFormatter);
                    var requestInfo = new RequestInfo(RequestType.Generic,
                         paramInfos,
                         typePair.ResponseType,
                         requestNameFormatter.GetFormattedName(typePair.RequestType),
                         responseNameFormatter.GetFormattedName(typePair.ResponseType)
                         );
                    requestInfos.Add(requestInfo);
                    requestInfoTypeStorage.AddRequest(requestInfo, typePair.RequestType);
                }
                foreach (var commandType in domainOption.CommandTypes)
                {
                    List<Application.Initialization.ParameterInfo> paramInfos = TypedProviderHelper.HarversParameterInfos(commandType, parameterNameFormatter);
                    var requestInfo = new RequestInfo(RequestType.Command, paramInfos, requestNameFormatter.GetFormattedName(commandType));
                    requestInfos.Add(requestInfo);
                    requestInfoTypeStorage.AddRequest(requestInfo, commandType);
                }
                foreach (var typePair in domainOption.CommandWithResponseTypes)
                {
                    List<Application.Initialization.ParameterInfo> paramInfos = TypedProviderHelper.HarversParameterInfos(typePair.RequestType, parameterNameFormatter);
                    var requestInfo = new RequestInfo(RequestType.Command,
                        paramInfos,
                        typePair.ResponseType,
                        requestNameFormatter.GetFormattedName(typePair.RequestType),
                        responseNameFormatter.GetFormattedName(typePair.ResponseType)
                        );
                    requestInfos.Add(requestInfo);
                    requestInfoTypeStorage.AddRequest(requestInfo, typePair.RequestType);
                }

                foreach (var typePair in domainOption.QueryTypes)
                {
                    List<Application.Initialization.ParameterInfo> paramInfos = TypedProviderHelper.HarversParameterInfos(typePair.RequestType, parameterNameFormatter);
                    var requestInfo = new RequestInfo(RequestType.Query,
                         paramInfos,
                         typePair.ResponseType,
                         requestNameFormatter.GetFormattedName(typePair.RequestType),
                         responseNameFormatter.GetFormattedName(typePair.ResponseType)
                         );
                    requestInfos.Add(requestInfo);
                    requestInfoTypeStorage.AddRequest(requestInfo, typePair.RequestType);
                }
                domains.Add(new DomainInfo(domainOption.DomainName, requestInfos));
            }

            return domains;
        }
    }
}