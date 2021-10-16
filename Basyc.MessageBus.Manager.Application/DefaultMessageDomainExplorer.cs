using Kontrer.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application
{
    public class DefaultMessageDomainExplorer : IMessageExplorer
    {
        private readonly Type iQueryType;
        private readonly Type iCommandType;
        private readonly Type iCommandWithResponseType;

        public DefaultMessageDomainExplorer(Type iQueryType, Type iCommandType, Type iCommandWithResponseType)
        {
            this.iQueryType = iQueryType;
            this.iCommandType = iCommandType;
            this.iCommandWithResponseType = iCommandWithResponseType;
        }

        public List<MessageDomainInfo> FindMessageDomains(params Assembly[] assembliesWithMessages)
        {
            var domains = new List<MessageDomainInfo>();

            foreach (var assemblyWithMessages in assembliesWithMessages)
            {
                var messageInfos = new List<RequestInfo>();
                foreach (var type in assemblyWithMessages.GetTypes())
                {
                    if (type.IsClass is false)
                        continue;
                    if (type.IsAbstract is true)
                        continue;

                    if (type.GetInterface(iQueryType.Name) is not null)
                    {
                        var responseType = GenericsHelper.GetGenericArgumentsFromParent(type, iQueryType)[0];
                        var paramInfos = type.GetConstructors()
                            .First()
                            .GetParameters()
                            .Select(paramInfo => new RequestParameterInfo(paramInfo.ParameterType, paramInfo.Name))
                            .ToList();
                        messageInfos.Add(new RequestInfo(type, false, paramInfos, responseType));
                        continue;
                    }

                    if (type.GetInterface(iCommandWithResponseType.Name) is not null)
                    {
                        var responseType = GenericsHelper.GetGenericArgumentsFromParent(type, iCommandWithResponseType)[0];
                        var paramInfos = type.GetConstructors()
                            .First()
                            .GetParameters()
                            .Select(paramInfo => new RequestParameterInfo(paramInfo.ParameterType, paramInfo.Name))
                            .ToList();
                        messageInfos.Add(new RequestInfo(type, false, paramInfos, responseType));
                        continue;
                    }

                    if (type.GetInterface(iCommandType.Name) is not null)
                    {
                        var paramTypes = type.GetConstructors().First().GetParameters().Select(x => x.ParameterType).ToList();
                        var paramInfos = type.GetConstructors()
                            .First()
                            .GetParameters()
                            .Select(paramInfo => new RequestParameterInfo(paramInfo.ParameterType, paramInfo.Name))
                            .ToList();
                        messageInfos.Add(new RequestInfo(type, true, paramInfos));
                        continue;
                    }
                }

                domains.Add(new MessageDomainInfo(assemblyWithMessages.GetName().Name, messageInfos));
            }
            return domains;
        }
    }
}