using System.Reflection;

namespace Basyc.MessageBus.Manager.Application
{
    public interface IDomainNameFormatter
    {
        string GetFormattedName(Assembly assembly);
    }
}