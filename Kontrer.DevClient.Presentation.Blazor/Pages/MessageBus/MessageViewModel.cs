using System;
using System.Collections.Generic;

namespace Kontrer.DevClient.Presentation.Blazor.Pages.MessageBus
{
    public record MessageViewModel(Type type, List<Type> parameters);
}