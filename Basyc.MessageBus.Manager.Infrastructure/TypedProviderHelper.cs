using Basyc.MessageBus.Manager.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Infrastructure
{
    public static class TypedProviderHelper
    {
        public static List<Application.Initialization.ParameterInfo> HarversParameterInfos(Type requestType, ITypedParameterNameFormatter parameterNameFormatter)
        {
            return requestType.GetConstructors()
                .First()
                .GetParameters()
                .Select(paramInfo => new Application.Initialization.ParameterInfo(paramInfo.ParameterType, paramInfo.Name, parameterNameFormatter.GetCustomTypeName(paramInfo.ParameterType)))
                .ToList();
        }
    }
}