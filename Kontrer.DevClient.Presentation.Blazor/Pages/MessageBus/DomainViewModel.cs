using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.DevClient.Presentation.Blazor.Pages.MessageBus
{
    public record DomainViewModel(string DomainName, List<MessageViewModel> Messages);
}