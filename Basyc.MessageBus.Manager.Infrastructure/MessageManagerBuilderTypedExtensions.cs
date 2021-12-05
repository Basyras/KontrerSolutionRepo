using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Infrastructure
{
    public static class MessageManagerBuilderTypedExtensions
    {
        public static TypedProviderBuilder UseTypedProvider(this BusManagerBuilder managerBuilder)
        {
            managerBuilder.AddProvider<TypedDomainProvider>();
            return new TypedProviderBuilder(managerBuilder.services);
        }
    }
}