using System;
using System.Collections.Generic;
using System.Linq;

namespace Kontrer.DevClient.Presentation.Blazor.Pages.MessageBus
{
    public class MessageViewModel
    {
        public MessageViewModel(Type type, List<Type> parameters, bool hasResponse, Type responseType = null)
        {
            Type = type;
            Parameters = parameters;
            HasResponse = hasResponse;
            ParametersValues = Enumerable.Range(0, parameters.Count).Select(x => String.Empty).ToList();
            ResponseType = responseType;
        }

        public Type Type { get; }
        public List<Type> Parameters { get; }
        public bool HasResponse { get; }
        public Type ResponseType { get; }
        public List<string> ParametersValues { get; }
    }
}