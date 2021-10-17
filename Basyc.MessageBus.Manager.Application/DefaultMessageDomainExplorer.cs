using Kontrer.Shared.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application
{
    public class DefaultMessageDomainExplorer : IMessageExplorer
    {
        private readonly IParameterTypeNameFormatter typeNameFormatter;
        private readonly IDomainNameFormatter domainNameFormatter;
        private readonly IRequestNameFormatter requestNameFormatter;
        private readonly IOptions<DefaultMessageDomainExplorerOptions> options;

        public DefaultMessageDomainExplorer(
            IParameterTypeNameFormatter typeNameFormatter, 
            IDomainNameFormatter domainNameFormatter,
            IRequestNameFormatter requestNameFormatter,
            IOptions<DefaultMessageDomainExplorerOptions> options
            
            )
        {
            this.typeNameFormatter = typeNameFormatter;
            this.domainNameFormatter = domainNameFormatter;
            this.requestNameFormatter = requestNameFormatter;
            this.options = options;
        }

        public List<MessageDomainInfo> FindMessageDomains(params Assembly[] assembliesWithMessages)
        {
            var domains = new List<MessageDomainInfo>();

            foreach (var assemblyWithMessages in assembliesWithMessages)
            {
                var requestInfos = new List<RequestInfo>();
                foreach (var type in assemblyWithMessages.GetTypes())
                {
                    if (type.IsClass is false)
                        continue;
                    if (type.IsAbstract is true)
                        continue;

                    if (type.GetInterface(options.Value.IQueryType.Name) is not null)
                    {
                        var responseType = GenericsHelper.GetGenericArgumentsFromParent(type, options.Value.IQueryType)[0];
                        var paramInfos = type.GetConstructors()
                            .First()
                            .GetParameters()
                            .Select(paramInfo => new RequestParameterInfo(paramInfo.ParameterType, paramInfo.Name, typeNameFormatter.GetCustomTypeName(paramInfo.ParameterType)))
                            .ToList();
                        requestInfos.Add(new RequestInfo(type, false, paramInfos, responseType, requestNameFormatter.GetFormattedName(type)));
                        continue;
                    }

                    if (type.GetInterface(options.Value.ICommandWithResponseType.Name) is not null)
                    {
                        var responseType = GenericsHelper.GetGenericArgumentsFromParent(type, options.Value.ICommandWithResponseType)[0];
                        var paramInfos = type.GetConstructors()
                            .First()
                            .GetParameters()
                            .Select(paramInfo => new RequestParameterInfo(paramInfo.ParameterType, paramInfo.Name, typeNameFormatter.GetCustomTypeName(paramInfo.ParameterType)))
                            .ToList();
                        requestInfos.Add(new RequestInfo(type, false, paramInfos, responseType, requestNameFormatter.GetFormattedName(type)));
                        continue;
                    }

                    if (type.GetInterface(options.Value.ICommandType.Name) is not null)
                    {
                        var paramTypes = type.GetConstructors().First().GetParameters().Select(x => x.ParameterType).ToList();
                        var paramInfos = type.GetConstructors()
                            .First()
                            .GetParameters()
                            .Select(paramInfo => new RequestParameterInfo(paramInfo.ParameterType, paramInfo.Name, typeNameFormatter.GetCustomTypeName(paramInfo.ParameterType)))
                            .ToList();
                        requestInfos.Add(new RequestInfo(type, true, paramInfos, requestNameFormatter.GetFormattedName(type)));
                        continue;
                    }
                }

                domains.Add(new MessageDomainInfo(domainNameFormatter.GetFormattedName(assemblyWithMessages), requestInfos));
            }
            return domains;
        }
    }
}