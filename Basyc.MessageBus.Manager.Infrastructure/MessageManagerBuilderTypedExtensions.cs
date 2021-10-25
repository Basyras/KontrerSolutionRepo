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
        public static TypedProviderBuilder UseTypedProvider(this MessageManagerBuilder managerBuilder)
        {
            //managerBuilder.services.Configure<TypedDomainProviderOptions>(options =>
            //{
            //});
            managerBuilder.UseProvider<TypedDomainProvider>();
            return new TypedProviderBuilder(managerBuilder.services);
        }
    }
}