using Basyc.MessageBus.Manager.Application.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application
{
    public class Parameter
    {
        public Parameter(ParameterInfo requestInfo, object value)
        {
            RequestInfo = requestInfo;
            Value = value;
        }

        public ParameterInfo RequestInfo { get; init; }
        public object Value { get; init; }
    }
}