using System;

namespace Basyc.MessageBus.Manager.Application
{
    public interface IRequestNameFormatter
    {
        string GetFormattedName(Type request);
    }
}